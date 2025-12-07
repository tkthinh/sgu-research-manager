# Scientific Evidence Management – Backend Agent Spec

## System Overview
This backend is a .NET 9 Web API for managing scientific research evidence and workloads for university faculty. It supports submission and maintenance of research works, author registrations for workload conversion, rule-based scoring, academic-year-based open/close windows, department- and role-based management, notifications, and Excel-based import/export.

## Architecture
- Solution: `Api.sln` with four main projects:
  - `Domain/`: entities, enums, core interfaces (`IGenericRepository<T>`, `IGenericService<TDto>`, `IUnitOfWork`, `IGenericMapper<TDto,T>`, plus `IUserRepository`, `IWorkRepository`, etc.).
  - `Application/`: feature folders (Departments, Works, Users, SystemConfigs, AcademicYears, Authors, AuthorRegistrations, AuthorRoles, Assignments, Factors, Fields, Purposes, SCImagoFields, WorkLevels, WorkTypes, Notifications, Auth, Caches) containing DTOs, mappers, and services.
  - `Infrastructure/`: EF Core contexts (`ApplicationDbContext`, `AuthDbContext`), generic/specialized repositories, `UnitOfWork`, Identity services, Redis, DI wiring, and seeding.
  - `WebApi/`: controllers, `Program.cs`, `ExceptionMiddleware`, `NotificationHub` (SignalR), health checks, static files.
- Dependency direction: WebApi -> Application -> Domain; Infrastructure implements Domain interfaces and is registered via `AddInfrastructure` and consumed by WebApi.
- Patterns:
  - Generic repository + unit of work for persistence.
  - `GenericService<TDto,T>` and `GenericCachedService<TDto,T>` for CRUD and caching.
  - DTO + mapper per entity (`XxxDto`, `CreateXxxRequestDto`, `UpdateXxxRequestDto`, `XxxMapper`).
  - Standard `ApiResponse<T>` envelope and global exception middleware.

## Domain Model (Key Entities)
- `BaseEntity`: `Id`, `CreatedDate`, `ModifiedDate`.
- `Department`: `Name` + `Users`, `Assignments`. Departments group users and manager assignments.
- `Field`: `Name` + `Users`. Academic/subject fields.
- `User`: core faculty/staff record with `FullName`, `UserName`, `Email`, `PhoneNumber`, `Specialization`, `AcademicTitle`, `OfficerRank`, `IdentityId` (links to Identity user), `DepartmentId`, `FieldId`, and navs to `Department`, `Field`, `Assignments`, `Authors`.
- `Assignment`: links `UserId` and `DepartmentId` (e.g., managers to departments).
- `WorkType`: `Name` and collections of `AuthorRoles`, `Factors`, `WorkLevels`, `Purposes`, `SCImagoFields`. Some WorkType IDs are hard-coded for special rules (e.g., projects, scientific articles, conferences).
- `WorkLevel`: `Name`, `WorkTypeId` + `WorkType`, `Factors`. Represents level (e.g., international vs national).
- `AuthorRole`: `Name`, `IsMainAuthor`, `WorkTypeId` + `WorkType`, `Authors`, `Factors`. Distinguishes main vs regular author workload.
- `SCImagoField`: `Name`. Classification used on Authors/Works.
- `Purpose`: `Name`, `WorkTypeId` + `WorkType`, `Authors`, `Factors`. Represents conversion purpose categories (duty, over-limit, research, etc.).
- `Factor`: `(WorkTypeId, WorkLevelId?, PurposeId, AuthorRoleId?, ScoreLevel?)`, `Name`, `ConvertHour`, `MaxAllowed?` with navs to `WorkType`, `WorkLevel`, `Purpose`, `AuthorRole`. This is the key rule table for conversion hours and per-user limits.
- `AcademicYear`: `Name`, `StartDate`, `EndDate` + `SystemConfigs`. Determines current year and registration windows.
- `SystemConfig`: `Name`, `OpenTime`, `CloseTime`, `IsDeleted`, `IsNotified`, `AcademicYearId` + `AcademicYear`. Global query filter hides `IsDeleted` items. Used as system window for open/closed state.
- `Work`: `Title`, `TimePublished?`, `TotalAuthors?`, `TotalMainAuthors?`, `Details : Dictionary<string,string>?` (stored as JSON), `Source : WorkSource`, `IsLocked`, `WorkTypeId`, `WorkLevelId?`, `AcademicYearId`, `ExchangeDeadline?` (typically `TimePublished + 18 months` for user-declared works) + navs to `WorkType`, `WorkLevel`, `AcademicYear`, `Authors`, `WorkAuthors`.
- `Author`: links `WorkId`, `UserId`, `AuthorRoleId?`, `PurposeId`, `SCImagoFieldId?`, `FieldId?`, `Position?`, `ScoreLevel?`, `AuthorHour : decimal`, `WorkHour : int`, `ProofStatus`, `Note?` + navs to `Field`, `SCImagoField`, `User`, `AuthorRole`, `Work`, `Purpose`, `WorkAuthors`, `AuthorRegistration`.
- `AuthorRegistration`: `AcademicYearId`, `AuthorId` + navs to `AcademicYear`, `Author`. Unique on `(AcademicYearId, AuthorId)` and one-to-one with `Author`. Indicates an author has registered that work for conversion in that year.
- `WorkAuthor`: `WorkId`, `UserId` + navs to `Work`, `User`. Additional co-author link used for notifications and filtering.
- `Notification`: `Content`, `IsGlobal`, `UserId?`, `IsRead?`. Persistent notification store.
- Enums:
  - `AcademicTitle`, `OfficerRank` (stored as string on User).
  - `WorkSource` (`QuanLyNhap`, `NguoiDungKeKhai`, stored as string on Work).
  - `ProofStatus` (`HopLe`, `KhongHopLe`, `ChuaXuLy`, stored as string on Author; central to validation rules).
  - `ScoreLevel` and `Achievement`: granular achievement/score level enums, used together with `Factor` to determine conversion hours.

## Application Layer (Services & Use Cases)
- Shared:
  - `GenericService<TDto,T>`: basic CRUD via `IUnitOfWork` and `IGenericMapper`.
  - `GenericCachedService<TDto,T>`: CRUD with Redis caching (`IDistributedCache`), safe cache failure handling, and invalidation helpers. Uses `cacheKeyPrefix = typeof(T).Name.ToLower()` plus suffixes (`_all`, `_id`, etc.).
  - `CurrentUserService`: reads claims (`"id"`, `"fullName"`/`ClaimTypes.Name`) via `IHttpContextAccessor`, returning `(isSuccess, userId, userName)`.
  - `ApiResponse<T>`: standard response envelope with `Success`, `Message`, and `Data`.
  - `ErrorMessages`: localized strings used by Work/User services (system closed messages, work/author errors, factor not found, score limit exceeded, etc.).

- System state & academic years:
  - `SystemConfigService`:
    - Extends `GenericCachedService<SystemConfigDto,SystemConfig>` with includes and custom cache.
    - `GetSystemState()` / `IsSystemOpenAsync(DateTime)` / `GetCurrentActiveSystemConfig()` determine whether system is currently open based on non-deleted configs where `OpenTime <= now <= CloseTime`.
    - `UpdateAsync` disallows updates once `IsNotified` is true.
    - `DeleteAsync` soft-deletes configs and blocks deletion if `IsNotified` is true.
    - `NotifySystemOpening` sends a global notification via `NotificationService`, marks `IsNotified` true, and saves.
  - `AcademicYearService`:
    - Extends `GenericCachedService<AcademicYearDto,AcademicYear>`.
    - `GetCurrentAcademicYear()` uses `StartDate <= today <= EndDate`, caches as `academic_year_current`.

- Work lifecycle & rules:
  - `WorkService` (extends `GenericCachedService<WorkDto,Work>`, implements `IWorkService`):
    - `CreateWorkWithAuthorAsync(CreateWorkRequestDto)`:
      - Requires system open (`SystemConfigService.IsSystemOpenAsync`), otherwise throws `SystemClosedNewWork`.
      - Rejects duplicate works by `(Title, WorkTypeId, WorkLevelId)`.
      - Creates `Work` with `Source = NguoiDungKeKhai`, `ExchangeDeadline = TimePublished + 18 months` (if `TimePublished` present), `IsLocked = false`, and `CreatedDate = UtcNow`.
      - Resolves `Factor` via `WorkCalculateService.FindFactorAsync`; throws `FactorNotFound` if not found.
      - Computes `WorkHour` via `WorkCalculateService.CalculateWorkHour`.
      - Uses `CurrentUserService` to get `UserId`; throws `UserIdNotDetermined` if missing.
      - Creates `Author` for the current user with appropriate role/purpose/score fields and computed hours.
      - Saves via `UnitOfWork` and invalidates caches.
      - Special logic: for a specific project WorkType ID, automatically creates a linked “scientific article” work using `CreateScientificArticleFromProjectAsync` (new Work + Author + WorkAuthor with project details copied).
    - `DeleteWorkAsync(Guid workId, Guid userId, ...)`:
      - Checks system open (`SystemClosedDeleteWork`).
      - Loads work with authors, deletes all `Author` records and the `Work`, and invalidates caches.
      - Controllers enforce additional constraints (only author, proof-status checks) before calling this service.
    - `RegisterWorkByAuthorAsync(Guid authorId, bool registered, ...)`:
      - Enforces system open (`SystemClosedMarkingWork`).
      - Loads Author + Work; gets current academic year.
      - When registering (`registered == true`):
        - Uses `WorkCalculateService.FindFactorAsync` to get Factor for `(workType,workLevel,purpose,authorRole,scoreLevel)`.
        - Counts existing `AuthorRegistration` records for same `User`, `AcademicYear`, `Purpose`, `ScoreLevel`.
        - If `Factor.MaxAllowed` is set and count >= `MaxAllowed`, throws `ScoreLimitExceeded` (formatted with work type name, level name, and score level).
        - Inserts `AuthorRegistration` if within limit.
      - When unregistering, removes relevant `AuthorRegistration`.
    - `UpdateWorkByAdminAsync` / `UpdateWorkByAuthorAsync`:
      - Encapsulate complex rules around editing works and authors:
        - Admin path can update work fields (including academic year, after verifying target year exists) and re-score authors.
        - Author path enforces:
          - Only authors/co-authors (via Authors or WorkAuthors) can update.
          - If current author has `ProofStatus.HopLe`, they cannot change their own info.
          - If any author has `ProofStatus.HopLe`, authors cannot change Work-level information; the request is transformed to author-only updates.
          - Authors cannot change `AcademicYearId`; it is nulled on incoming request.
        - Both paths use `WorkCalculateService.ShouldRecalculateAuthorHours` and `CalculateAuthorHour` where necessary.
      - Both send notifications (via `NotificationService` + SignalR) to affected authors/co-authors when changes occur.

  - `WorkQueryService` (extends `GenericCachedService<WorkDto,Work>`, implements `IWorkQueryService`):
    - `GetWorkByIdWithAuthorsAsync(Guid)` loads a work + authors via `IWorkRepository.GetWorkWithAuthorsByIdAsync`, maps to `WorkDto`, and populates `CoAuthorUserIds` (combining Authors and WorkAuthors, excluding current user when appropriate), caching by key `work_with_authors_{id}`.
    - `GetWorksAsync(WorkFilter filter)`:
      - If `filter.IsCurrentUser` and `UserId` is null, injects current user from `CurrentUserService`.
      - Builds an EF expression predicate using `BuildWorkPredicate`, combining filters on:
        - User scope (authors and/or co-authors via WorkAuthors; includes special cases for `OnlyRegisteredWorks` vs `OnlyRegisterableWorks` using `AuthorRegistration` and `ExchangeDeadline`).
        - Department (`Author.User.DepartmentId`).
        - Academic year, `ProofStatus`, `WorkSource`.
      - Executes via `IWorkRepository.GetWorksByFilterAsync`.
      - Post-processes results (`ProcessWorksResult`) and fills `CoAuthorUserIds` efficiently for all returned works.

  - `WorkCalculateService` (`IWorkCalculateService`):
    - `CalculateWorkHour(ScoreLevel? scoreLevel, Factor factor)`:
      - Returns `factor.ConvertHour` (also in cases of mismatched score levels), 0 when factor is null.
    - `CalculateAuthorHour(int workHour, int totalAuthors, int totalMainAuthors, Guid? authorRoleId, ...)`:
      - Resolves `AuthorRole`; throws if not found.
      - Detects a special “conference” WorkType ID; for such works returns `workHour` directly.
      - Otherwise, validates totals, then computes:
        - For main authors: `(1/3 * workHour / totalMainAuthors) + (2/3 * workHour / totalAuthors)`.
        - For regular authors: `2/3 * workHour / totalAuthors`.
      - Rounds to 1 decimal place.
    - `FindFactorAsync(...)`:
      - Filters `Factor` by WorkType, Purpose, then applies optional filters for WorkLevel, AuthorRole, ScoreLevel, preferring exact `AuthorRoleId` matches but falling back when necessary.

- Authors and registrations:
  - `AuthorService` (extends `GenericCachedService<AuthorDto,Author>`):
    - `GetAllRegistableAuthorsOfUser(Guid userId, ...)`:
      - Loads Authors including Work, roles, purposes, fields, AuthorRegistration, and related AcademicYear.
      - Filters to:
        - `Author.UserId == userId`.
        - `Work.ExchangeDeadline >= today`.
        - `AuthorRegistration == null` or `AcademicYearId == Guid.Empty` (not registered yet).
      - This returns authors eligible for registration in the current window.
  - `AuthorRegistrationService` uses generic patterns for CRUD around `AuthorRegistration` entries.

- Users:
  - `UserService` (extends `GenericCachedService<UserDto,User>`, uses `IUserRepository`):
    - Overrides `GetByIdAsync` and `GetAllAsync` to load department and field details and apply custom caching (~30 minutes).
    - `GetUserByIdentityIdAsync` maps Identity ID (`ApplicationUser.Id`) to domain user.
    - `SearchUsersAsync(searchTerm)` performs simple term-based search.
    - `GetUsersByDepartmentIdAsync(departmentId)` returns users for a department.
    - `GetUserConversionResultAsync(Guid userId)`:
      - Validates non-empty `userId`.
      - Resolves current academic year; throws if missing.
      - Loads `Purpose` records and partitions them into categories (duty, over-limit, research) via configured IDs.
      - Queries `Author` entries for user with `ProofStatus.HopLe` and an attached `AuthorRegistration` for the current academic year.
      - If none, returns a default object with all totals zeroed.
      - Otherwise:
        - Groups works per category, counts unique work IDs, sums `AuthorHour` per category.
        - Applies an 80-hour cap for duty conversion.
        - Over-limit “calculated hours” use the subset of authors flagged as registered.
        - Research product conversion currently contributes 0 but is structured for future rules.
      - Returns `UserConversionResultRequestDto` with per-category and total stats.

- Catalog/config services (Departments, Purposes, WorkTypes, WorkLevels, Fields, AuthorRoles, Factors, SCImagoFields, ScoreLevels, AcademicYears, Assignments, Notifications) follow the same DTO-mapper-service pattern. Notable extras:
  - `DepartmentService.GetDepartmentsByManagerIdAsync(Guid managerId)` via `Assignment`.
  - `PurposeService.GetPurposesByWorkTypeIdAsync(Guid workTypeId)`.
  - `NotificationService` adds methods to create global/user notifications, get notifications with caching, and mark them as read.
  - `CacheManagementService` exposes key enumeration and clear-all operations via Redis.

- Auth & Identity:
  - `AuthService` handles login, registration, and password changes over ASP.NET Identity (`ApplicationUser`, `AuthDbContext`) and bridges to domain `User` (`UserService`).
  - `UserImportService` uses EPPlus to import users from Excel, creating Identity + domain users transactional and assigning roles.

## Persistence & Infrastructure
- `ApplicationDbContext` (EF Core, SQL Server):
  - DbSets for all domain entities.
  - Overrides `SaveChangesAsync` to set `CreatedDate` / `ModifiedDate`.
  - Configures JSON conversion for `Work.Details` with `ValueComparer`.
  - Defines all relationships, indices (especially for `Work`, `Author`, `WorkAuthor`, `AuthorRegistration`), and global query filter on `SystemConfig.IsDeleted`.
  - Converts enum properties (`Work.Source`, `User.AcademicTitle`, `User.OfficerRank`, `Author.ProofStatus`) to strings.
  - Applies seed configurations for fields, departments, work types/levels, author roles, purposes, SCImago fields, factors, academic years.

- `AuthDbContext` (IdentityDbContext<ApplicationUser>):
  - Identity store with extended `ApplicationUser.IsApproved`.

- `UnitOfWork` + `GenericRepository<T>`:
  - `UnitOfWork` caches repositories per entity type and delegates `SaveChangesAsync` to `ApplicationDbContext`.
  - `GenericRepository<T>` implements `IGenericRepository<T>` with standard EF patterns and does not commit on its own.
  - Specialized repositories (`UserRepository`, `WorkRepository`, `WorkTypeRepository`, `AssignmentRepository`) provide richer queries including navigation properties.

- DI: `DependencyInjection.AddInfrastructure`:
  - Configures both DbContexts with `UseSqlServer(DefaultConnection)`.
  - Configures Identity with simple password rules and EF stores.
  - Configures Redis (`AddStackExchangeRedisCache`) and registers `IConnectionMultiplexer` (logging and disabling when connection fails).
  - Configures JWT Bearer auth using `Jwt:Issuer`, `Jwt:Audience`, `Jwt:Key`, with `NameClaimType = "id"` and special handling for SignalR tokens on `/notification-hub`.
  - Registers all application services, mappers, repositories, `IUnitOfWork`, `ICurrentUserService`, `IAuthService`, `IUserImportService`, and `ICacheManagementService`.

- `AuthDbInitializer.SeedDataAsync` seeds roles (Admin/Manager/User) and default admin/manager identities plus corresponding domain users using `UserService`.

## API Surface (Highlights)
- Most controllers live in `WebApi/Controllers` and follow this pattern:
  - Route: `[Route("api/[controller]")]`.
  - Require auth globally; some actions mark `[AllowAnonymous]`.
  - Wrap results in `ApiResponse<T>`.
  - Use role-based `[Authorize(Roles = "...")]` for privileged operations.

Key controllers:
- `AuthController (api/auth)`:
  - `POST /login` (anonymous): returns JWT + user info.
  - `POST /register` (anonymous): creates Identity + domain user.
  - `POST /change-password`: authenticated user password change.

- `WorksController (api/works)`:
  - `GET /filter`: main endpoint for listing works using `WorkFilter` (supports user, department, academic year, proof status, source, onlyRegistered, onlyRegisterable, isCurrentUser). Uses `WorkQueryService`.
  - `GET /{id}`: work details with authors.
  - `POST /` (User): create new user-declared work + initial author; respects system open window and uniqueness rules.
  - `DELETE /{id}` (User): delete own work, blocked if any author is `ProofStatus.HopLe`; triggers notifications to others.
  - `PATCH /authors/{authorId}/register` (User): register/unregister author for conversion, enforcing `Factor.MaxAllowed` and system open.
  - `PATCH /{workId}/admin-update/{userId}` (Admin/Manager): admin scoring update and notification to specific author.
  - `PATCH /{workId}` (User): update work and/or author info, with strict checks based on authorship and `ProofStatus`.

- `ExcelController (api/excel)`:
  - `POST /import` (Admin/Manager): Excel import of works.
  - `GET /export-by-user`: authenticated user export of their works as Excel using `WorkFilter`.
  - `GET /export-by-admin`: export works for specific user.
  - `GET /export-all-works` (Admin/Manager): export across all works.

- `UsersController (api/users)`:
  - `GET /` (Admin/Manager): all users with roles and approval status.
  - `GET /{id}`: user details.
  - `PUT /{id}`: self-profile update.
  - Admin update endpoint for role/approval (pattern inferred from code, under same controller).
  - `GET /me`: current user info.
  - `DELETE /{id}`: delete user.
  - `GET /conversionresult/{userId}` (Authorized): user conversion summary; non-admins can only access their own.
  - `GET /search?searchTerm=...`: user search.
  - `GET /department/{departmentId}`: users for a department with role info.
  - `POST /import` (Admin): Excel-based user import.

- `SystemConfigsController (api/systemconfigs)`:
  - `GET /` (Admin): list configs.
  - `GET /check`: returns whether system is open; either `ApiResponse<SystemConfigDto>` or `ApiResponse<object>` with closed message.
  - `GET /year/{academicYearId}`: configs per year.
  - `POST /` (Admin): create config.
  - `POST /notify/{id}` (Admin): mark config as notified and send global notification.
  - `PUT /{id}` (Admin): update config; blocked once `IsNotified` is true.
  - `DELETE /{id}` (Admin): soft delete, blocked once `IsNotified` is true.

- `DepartmentsController (api/departments)`:
  - `GET /` (anonymous): all departments.
  - `GET /by-manager/{managerId}` (Manager): only allowed if `managerId` matches current user; returns assigned departments based on `Assignment`.
  - CRUD endpoints for Admin.

- `PurposesController`, `WorkTypesController`, `WorkLevelsController`, `FieldsController`, `AuthorRolesController`, `FactorsController`, `SCImagoFieldsController`, `ScoreLevelsController`, `ProofStatusesController`, `AcademicYearsController`, `AssignmentsController`:
  - Follow standard CRUD patterns with `ApiResponse<T>`, plus feature-specific read endpoints (e.g., purposes by WorkType).

- `NotificationsController (api/notifications)`:
  - `GET /global`: global notifications.
  - `GET /me`: current user’s notifications.
  - `PUT /{id}`: mark notification as read.

- `CacheController (api/cache)`:
  - `GET /keys` (Admin): list Redis cache keys.
  - `DELETE /clear/{key}` (Admin): remove specific key.
  - `DELETE /clear-all` (Admin): clear all cache keys.

- `FallbackController`: SPA fallback mapped via `MapFallbackToController("Index","Fallback")`.

## Cross-Cutting Concerns
- Logging:
  - Serilog configured in `Program.cs` and used for request logging.
  - Services/controllers use `ILogger<T>` for operational and error logging (especially import/export, caching, factor/score processing, and automatic article creation).

- Error handling:
  - `ExceptionMiddleware` wraps the pipeline and converts thrown `ValidationException`s into 400s and other exceptions into 500s using `ApiResponse<object>`.
  - Controllers commonly wrap service calls in try/catch to produce domain-specific 400/401/404 responses.

- Authentication & authorization:
  - JWT Bearer auth with tokens from `AuthService.LoginAsync`, including `"id"` and roles.
  - `Program.cs` adds a global `AuthorizeFilter` requiring authenticated users by default.
  - Controllers use `[AllowAnonymous]` for login/register and open department listing.
  - Role-based `[Authorize(Roles = "Admin|Manager|User")]` guard privileged endpoints.
  - Business logic also enforces ownership (e.g., only an author can delete their work, only managers can see their assigned departments, only self or admin can view conversion results).

- Caching:
  - Many catalog and read-heavy services extend `GenericCachedService`. Cache keys use consistent prefixes and TTLs.
  - On create/update/delete operations, services call `SafeInvalidateCacheAsync` to clear relevant keys.
  - `CacheManagementService` and `CacheController` allow administrators to inspect and clear caches.

- SignalR:
  - `NotificationHub` maps to `"notification-hub"` path; JWT tokens can be passed via `access_token` query when connecting.
  - `WorksController` uses `_hubContext<NotificationHub>` to push real-time notifications (e.g., when works are deleted or updated). System opening notifications use `NotificationService` and may be surfaced to clients via hub subscriptions.

## Conventions & Best Practices
- Keep to Clean Architecture:
  - Domain: entities, enums, interfaces only.
  - Application: DTOs, mappers, services; depend only on Domain abstractions.
  - Infrastructure: EF Core, Identity, repository/unit-of-work implementations, DI configuration.
  - WebApi: controllers, middleware, and hosting; depend on Application services and DI.

- Feature structure:
  - For new features, follow existing pattern (`Application/FeatureName` + `WebApi/Controllers/FeatureNameController.cs`).
  - Implement new DTO/mapper/service interfaces in `Application`, new repository interfaces in `Domain` as needed, and repository implementations in `Infrastructure`.

- System open/closed rules:
  - Always use `ISystemConfigService` (e.g., `IsSystemOpenAsync`) to enforce time windows for operations like creating works, registering for conversion, and deleting works.
  - Use `ErrorMessages.SystemClosed*` strings to keep error responses consistent.

- Proof status & workflow:
  - `ProofStatus.HopLe` is the key guard:
    - Authors cannot delete works with any `HopLe` author.
    - Authors’ ability to edit works is restricted once any author becomes `HopLe`.
    - Admin-level endpoints exist for scoring and classification when `ProofStatus` changes.

- Conversion logic:
  - Use `Factor` and `ScoreLevel` to define new conversion rules, and `WorkCalculateService` to compute `WorkHour` and `AuthorHour`.
  - Respect `Factor.MaxAllowed` when registering works per academic year to avoid exceeding allowed counts.
  - Update `UserService.GetUserConversionResultAsync` if additional categories or caps are introduced.

- Current user access:
  - Prefer `ICurrentUserService` within services instead of direct `HttpContext` access.
  - Controllers that need advanced access control may read claims directly but should generally delegate to services for domain decisions.

- Standard responses:
  - Controllers should return `ApiResponse<T>` for consistency, using `Success` and `Message` to convey status and `Data` for payloads.
  - When throwing exceptions, rely on `ExceptionMiddleware` or controller try/catch to wrap them appropriately.

## How to Use This Agent
- Use this file as the primary context for reasoning about the backend. When asking an agent for help:
  - Refer to entities by their Domain names (`Work`, `Author`, `Factor`, `SystemConfig`, etc.).
  - Specify which layer (Domain, Application, Infrastructure, WebApi) you want to modify.
  - Ask for changes in line with the existing patterns (DTO + Mapper + Service + Controller + Repository when needed).
  - Call out whether changes must respect system open/closed windows, proof status, conversion/limit rules, or caching.
- The agent should:
  - Avoid inventing entities or endpoints not present in this design.
  - Preserve the standardized `ApiResponse<T>` pattern and Serilog logging.
  - Use `ICurrentUserService`, `ISystemConfigService`, and `WorkCalculateService` where appropriate for new features.
