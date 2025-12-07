# Scientific Evidence Management – Frontend Agent Spec

This file describes how the React frontend for the SGU scientific evidence management system is structured and how an agent should reason when extending or modifying it.

---

## App Overview

The app is a React + TypeScript SPA that supports:

- **Users (lecturers)**:
  - Sign up/sign in, maintain their profile.
  - Declare research works and co-authors.
  - Register eligible works for workload conversion/scoring.
  - View reports and conversion summaries of their works.
- **Managers**:
  - See departments they are assigned to.
  - View users in those departments and review/score their works.
  - Access aggregated statistics and exports.
- **Admins**:
  - Manage users and assignments.
  - Configure domain catalogs (types, levels, purposes, factors, fields, departments, academic years).
  - Configure system open/close periods.
  - Manage caches and global statistics.

Core workflows:
- Authentication and session lifecycle (login, token storage, auto signout on expiration).
- Work lifecycle: create/update, register/unregister, review by manager/admin, status and note updates.
- Reporting & statistics: filter and export works by academic year, department, user, proof status, and source.
- System configuration: open/close windows that determine when works can be edited or deleted.

---

## Tech Stack & Libraries

- **Framework & tooling**
  - React 19, React DOM 19.
  - TypeScript 5.
  - Vite 6 (dev, build, preview).

- **UI**
  - MUI v6 (`@mui/material`, `@mui/icons-material`).
  - MUI X Data Grid (`@mui/x-data-grid`) for data tables.
  - MUI X Date Pickers for date inputs.
  - Toolpad Core (`@toolpad/core`):
    - `DashboardLayout`, `PageContainer` for layout.
    - `ReactRouterAppProvider` for navigation + session.
    - `Account` / `useSession` for account UI.

- **Data & state**
  - React Query v5 (`@tanstack/react-query`) for server state.
  - React Context (`AuthContext`) for auth & current user.
  - Local component state via React hooks.
  - Redux Toolkit is installed but not used.

- **Forms & validation**
  - `react-hook-form` + `zod` + `@hookform/resolvers/zod`.

- **HTTP & real-time**
  - Axios (`src/lib/api/api.ts`) with a shared client.
  - `@microsoft/signalr` for live notifications.

- **UX & utilities**
  - `react-toastify` for toasts.
  - `luxon` and `date-fns` (with Vietnamese locale) for date/time.

- **Environment**
  - `VITE_API_URL`: base URL for API.
  - `VITE_HUB_URL`: SignalR hub URL for notifications.

---

## Architecture & Project Structure

Top-level:

- `src/main.tsx`
  - Renders root React tree into `#root`.
  - Wraps app with:
    - `ThemeProvider` (MUI theme from `src/theme.ts`).
    - `AuthProvider` (`src/app/shared/contexts/AuthContext.tsx`).
    - `RouterProvider` using `router` from `src/app/routes/Router.tsx`.

- `src/theme.ts`
  - Minimal theme configured for Toolpad:
    - `cssVariables.colorSchemeSelector = 'data-toolpad-color-scheme'`.
    - `colorSchemes: { light: true, dark: true }`.

- Layout & shell (`src/app/layouts`)
  - `App.tsx`
    - Top-level shell:
      - `ReactRouterAppProvider` (Toolpad) with:
        - `navigation`: one of `NAVIGATION_USER`, `NAVIGATION_MANAGER`, `NAVIGATION_ADMIN` from `Navigation.tsx`.
        - `branding`: logo + title.
        - `session`: `CustomSession` containing `User`.
        - `authentication`: `signIn()` → `/dang-nhap`, `signOut()` clears localStorage and redirects.
      - `QueryClientProvider` with a `QueryClient` for React Query.
      - Global `ToastContainer`.
      - `Outlet` for nested routes.
      - `SessionExpiredDialog` and `ProfileIncompletedDialog`.
    - Session logic:
      - Reads `user` from `AuthContext`.
      - Derives navigation by `user.role` (`User`, `Manager`, `Admin`).
      - Uses `isUserProfileIncomplete(user)` to show profile update dialog if needed.
      - Reads `expiration` from localStorage and auto-signs out at expiry.

  - `Layout.tsx`
    - Wraps feature pages in `DashboardLayout` and `PageContainer`.
    - Injects `CustomAccount` into the toolbar (`toolbarAccount` slot).

  - `Navigation.tsx`
    - Exports `NAVIGATION_USER`, `NAVIGATION_MANAGER`, `NAVIGATION_ADMIN` (Toolpad `Navigation` objects).
    - Items use `segment` values that line up with route segments (e.g. `cong-trinh`, `dang-ky-quy-doi`, `bao-cao`, `thong-ke`, `quan-ly-tai-khoan`, `cai-dat`, `cau-hinh-he-thong`).

- Shared app modules (`src/app/shared`)
  - `contexts/AuthContext.tsx`
    - Holds `user: User | null`, `loading`, `setUser`, `signOut`, `refreshUserInfo`.
    - `refreshUserInfo`:
      - Gets token from localStorage.
      - Calls `getMyInfo()` from `usersApi`.
      - Decodes JWT via `jwtDecode` and sets `user.role` from the role claim.
    - `signOut` clears `token` & `expiration` and redirects to `/dang-nhap`.
  - `components/ProtectedRoute.tsx`
    - Guards routes:
      - Checks token + expiration in localStorage.
      - Uses `useAuth` for current `user` & `loading`.
      - If unauthenticated → `/dang-nhap`.
      - If `allowedRoles` set and `user.role` not allowed → `/unauthorized`.
      - Else renders `children` or `Outlet`.
  - Header components:
    - `CustomAccount.tsx`:
      - MUI `Box` with `NotificationDropdown` and Toolpad `Account`.
      - Provides custom `AccountInfo` as `popoverContent`.
    - `AccountInfo.tsx`:
      - Uses `useSession<CustomSession>()` to get `user`.
      - Shows name, field, username, email, academic title, officer rank, department.
      - Link to `/cap-nhat-thong-tin`.
      - Includes `SignOutButton`.
    - `NotificationDropdown.tsx`:
      - React Query `useQuery(["notifications"], getMyNotifications)`.
      - SignalR hub subscription:
        - Connects to `VITE_HUB_URL?access_token=<token>`.
        - Listens for `"ReceiveNotification"` and triggers `refetch()`.
      - Shows unread count with `Badge`.
      - Renders a `Menu` listing notifications with formatted time and “Mark as read” button (`markNotificationAsRead` + `invalidateQueries(["notifications"])`).
  - Shared dialogs (`components/dialogs`):
    - `ProfileIncompletedDialog`: prompts user to update profile, hidden on `/cap-nhat-thong-tin`.
    - `SessionExpiredDialog`: informs when session expired, triggers login redirect.
    - `WorkUpdateDialog`: tabs (work info & author info) around `WorkForm`.
      - Uses current `user` and `selectedWork` to disable work tab when locked and author already “valid”.
    - `WorkStatusDialog`: select `ProofStatus` and submit.
    - `WorkNoteDialog`: edit and submit a note.
  - Shared tables (`components/tables`):
    - `DataTable.tsx`: simple wrapper around MUI DataGrid with Vietnamese locale and standard pagination.
    - `GenericTable.tsx`: basic MUI `<Table>` wrapper for small, static tables.
    - `WorksCollapsibleTable.tsx`:
      - Renders works in a main row + collapsible detail row.
      - Delegates edit/delete enablement to `canEditWork`/`canDeleteWork` props.
      - Shows current author’s purpose, proof status with icons, and per-work details keyed from `Work.details`.

- Hooks (`src/hooks`)
  - `useSystemStatus.ts`
    - `useQuery(["systemConfig"], getSystemConfig)` to get current config.
    - Computes `isSystemOpen` using Luxon in "Asia/Saigon".
    - Provides:
      - `systemConfig`, `isLoading`, `isSystemOpen`.
      - `canEditWork(proofStatus, isLocked)` and `canDeleteWork(proofStatus, isLocked, hasOtherValidAuthors)`.
        - **Locked work**:
          - Current author `HopLe` → cannot edit/delete.
          - Current author `ChuaXuLy` or `KhongHopLe` → can edit/delete.
        - **Unlocked work**:
          - System closed (`!isSystemOpen`) → only `KhongHopLe` can be edited/deleted.
          - System open → any non-`HopLe` can be edited/deleted.
  - `useWorkData.ts` (`useWorkFormData`)
    - Loads reference data for work forms via React Query:
      - `["workTypes"]` → `getWorkTypes()`.
      - `["workLevels"]` → `getWorkLevels()`.
      - `["authorRoles"]` → `getAuthorRoles()`.
      - `["purposes"]` → `getPurposes()`.
      - `["scimagoFields"]` → `getScimagoFields()`.
      - `["fields"]` → `getFields()`.
    - Returns arrays + `isLoading`/`isDataReady`.
  - `useWorkDialogs.ts`
    - Single hook to manage:
      - Dialog state (`openUpdateDialog`, `openStatusDialog`, `openNoteDialog`).
      - `selectedWork`, `activeTab`, `newStatus`, `newNote`.
      - Mutations:
        - For authors: `updateWorkByAuthor`, `createWork`.
        - For admins/managers: `updateWorkByAdmin`.
        - Status and note updates: `updateWorkStatus`, `updateWorkNote`.
      - Invalidation:
        - Author page → invalidates `["works", "my-works"]`.
        - Admin/manager page → invalidates `["works", "user", userId]`.
      - Normalizes request payloads (dates, IDs, coAuthor lists, score levels).

- API layer (`src/lib/api`)
  - `api.ts`
    - Shared Axios client:
      - `baseURL: import.meta.env.VITE_API_URL`.
      - JSON `Content-Type`.
      - Adds `Authorization: Bearer <token>` header from localStorage.
  - Domain APIs:
    - `authApi.ts`: `signIn`, `signUp`, `changePassword`.
    - `usersApi.ts`: search, get, update (user/admin), delete, get by department, get my info, conversion results, Excel import.
    - `worksApi.ts`: filter (`getWorksWithFilter`), get by id, create, delete, update by admin/author, register author, update status, update note.
    - `assignmentApi.ts`: get, create, update, delete manager–department assignments.
    - `notificationsApi.ts`: global notifications, user notifications, mark as read.
    - Catalog & config APIs: `academicYearApi.ts`, `departmentsApi.ts`, `fieldsApi.ts`, `workTypesApi.ts`, `workLevelsApi.ts`, `authorRolesApi.ts`, `purposesApi.ts`, `scimagoFieldsApi.ts`, `factorsApi.ts`, `scoreLevelsApi.ts`, `systemConfigApi.ts`, `cachesApi.ts`.
    - Excel APIs (`excelApi.ts`): export works (current user, by admin, or all), import works from Excel.

- Types & utilities (`src/lib/types`, `src/lib/utils`)
  - Typed models (e.g. `Work`, `User`, `AcademicYear`, `SystemConfig`, etc.).
  - Enums (`ProofStatus`, `ScoreLevel`, `WorkSource`, `Role`, `AcademicTitle`, `OfficerRank`).
  - Label mapping helpers: `academicTitleMap`, `officerRankMap`, `roleMap`.
  - Score helpers: `scoreLevelUtils` (`getScoreLevelText`, `getScoreLevelOptions`, `getScoreLevelFullDescription`).
  - Profile completeness: `checkUserProfile.ts`.
  - Time helpers: `dateTimeFormatter` (Luxon), `dateUtils` (date-fns).
  - `workDetailsConfig.ts`: defines dynamic `Work.details` fields per work type/level.

---

## Routing & Navigation

Router is defined in `src/app/routes/Router.tsx` using `createBrowserRouter`. Main route tree:

- Root:
  - `element: <App />`
  - Children:
    - `element: <ProtectedRoute />`
      - Children:
        - Path: `/`
          - `Component: Layout`
          - Children:
            - `/` → `Dashboard` (all roles).
            - `/cong-trinh` → `WorkPage` (roles: `User`, `Manager`).
            - `/dang-ky-quy-doi` → `WorkRegisterPage` (role: `User`).
            - `/cham-diem` → `WorkScorePage` (roles: `Manager`, `Admin`).
            - `/cham-diem/user/:userId` → `WorkScoreDetailPage` (roles: `Manager`, `Admin`).
            - `/phan-cong` → `AssignmentPage` (role: `Admin`).
            - `/cap-nhat-thong-tin` → `UpdateInfoPage` (any authenticated user).
            - `/test` → `TestPage` (all roles).
            - `/bao-cao` → `ReportPage` (roles: `User`, `Manager`).
            - `/thong-ke` → `StatisticsPage` (roles: `Admin`, `Manager`).
            - `/quan-ly-tai-khoan` → `UserPage` (role: `Admin`).
            - `/cau-hinh-he-thong` → `SystemConfigPage` (role: `Admin`).
            - `/cai-dat` → `Setting` (role: `Admin`).
            - `/cai-dat/loai-cong-trinh` → `WorkTypePage` (Admin).
            - `/cai-dat/cap-cong-trinh` → `WorkLevelPage` (Admin).
            - `/cai-dat/vai-tro-tac-gia` → `AuthorRolePage` (Admin).
            - `/cai-dat/muc-dich-quy-doi` → `PurposePage` (Admin).
            - `/cai-dat/he-so-quy-doi` → `FactorPage` (Admin).
            - `/cai-dat/nganh-scimago` → `ScimagoFieldPage` (Admin).
            - `/cai-dat/don-vi` → `DepartmentPage` (Admin).
            - `/cai-dat/nganh` → `FieldPage` (Admin).
            - `/cai-dat/nam-hoc` → `AcademicYearPage` (Admin).
            - `/cai-dat/quan-ly-cache` → `CachePage` (Admin).

- Public routes:
  - `/dang-ky` → `SignUp`.
  - `/dang-nhap` → `SignIn`.
  - `/unauthorized` → `Unauthorized`.
  - `*` → `NotFound`.

Toolpad navigation (`Navigation.tsx`) maps to these paths via `segment`. When adding routes:

- Update `Router.tsx` with desired `ProtectedRoute` and role guards.
- Add corresponding `segment` entry to the relevant `NAVIGATION_*` array.

---

## Data Fetching & Mutations (React Query)

Global:

- `App.tsx` instantiates a `QueryClient` and wraps the app in `QueryClientProvider`. No custom global defaults; per-query options are used.

Typical query patterns:

- `useQuery({ queryKey, queryFn, enabled?, staleTime? })`:
  - Works:
    - `["works", "my-works"]`: current user’s works in current academic year.
    - `["works", "registerable-works"]`: same, but used in registration view.
    - `["works", "user", userId]`: target user’s works (admin/manager).
  - System config:
    - `["systemConfig"]`: active config (for `useSystemStatus`).
    - `["system-configs"]`: configs for current academic year (admin settings).
  - Users:
    - `["user"]`: current user (`getMyInfo`).
    - `["users"]`: all users (admin).
    - `["users", userId]`: user by ID.
    - `["users", "department", departmentId]`: users by department.
  - Departments:
    - `["departments", role, userId]`: either all or by manager, depending on role.
  - Notifications:
    - `["notifications"]`: current user notifications.
    - `["global-notifications"]`: global notifications for dashboard.
  - Settings:
    - `["academic-years"]`, `["factors", workTypeId]`, `["workTypes"]`, `["workLevels"]`, `["fields"]`, `["purposes"]`, `["authorRoles"]`, `["scimagoFields"]`, `["assignments"]`, `["cache-keys"]`, etc.

Mutation patterns:

- Always use `useMutation` with:
  - `mutationFn` calling the typed API.
  - `onSuccess`:
    - Show `toast.success()` with a Vietnamese message.
    - Invalidate related queries (e.g. `invalidateQueries(["works", "my-works"])`).
    - Optionally close dialogs and/or `refetch` after a short delay.
  - `onError`:
    - Show `toast.error()` with error message.

Common invalidation patterns:

- Works:
  - Author pages (`WorkPage`, `WorkRegisterPage`):
    - On create/update/delete: invalidate `["works", "my-works"]` or `["works", "registerable-works"]`.
  - Scoring pages (`WorkScoreDetailPage`):
    - On update/status/note change: invalidate `["works", "user", userId]`.

- Users:
  - On delete or Excel import: invalidate `["users"]`.

- Assignments:
  - On create/update/delete: invalidate `["assignments"]`.

- System configs:
  - On create/update/delete/notify: invalidate `["system-configs"]`.

- Caches:
  - On delete/clear-all: invalidate `["cache-keys"]`.

When adding new functionality, follow this pattern:

- Define API call in `src/lib/api`.
- Use `useQuery` or `useMutation` in feature components.
- In `onSuccess`, invalidate all affected query keys and, if needed, update local state for immediate UI responsiveness.

---

## Key Screens & Workflows (High-Level)

### Auth

- `SignIn` (`/dang-nhap`):
  - `react-hook-form` + `zod` for `username` & `password`.
  - Calls `signIn`, stores `token` and `expiration`, then `refreshUserInfo()` and navigates to `/`.
  - Shows success alert if `?registered=true` in URL.

- `SignUp` (`/dang-ky`):
  - 2-step form (account + profile).
  - Validates fields like username (length), passwords, email, phone, academic title, officer rank, department, and field.
  - Uses `getDepartments` & `getFields` and enum label helpers to populate selects.

- `UpdateInfoPage` (`/cap-nhat-thong-tin`):
  - Loads current user via `getMyInfo`.
  - Allows editing of profile data (department, field, academic title, officer rank, contact info, specialization).
  - Has a separate “Change password” form calling `changePassword`.
  - Used together with `ProfileIncompletedDialog` to force completing profile on first login.

### Works – Author-Facing

- `WorkPage` (`/cong-trinh`):
  - Shows current user’s works in current academic year (`getWorksWithFilter({ academicYearId, isCurrentUser: true })`).
  - Uses `useSystemStatus` to:
    - Determine if system is open/closed (`isSystemOpen`).
    - Compute `canEditWork` / `canDeleteWork` for each row.
  - `WorksCollapsibleTable` displays works with per-author proof status, score level, and locking behavior.
  - “Thêm công trình” button opens `WorkUpdateDialog` (disabled when system closed).

- `WorkRegisterPage` (`/dang-ky-quy-doi`):
  - Lists works that the current user can register or unregister.
  - Uses `registerWorkByAuthor(authorId, registered)` with optimistic updates.
  - Shows registration state per author in DataGrid actions.

### Works – Manager/Admin Scoring

- `WorkScorePage` (`/cham-diem`):
  - Manager/Admin chooses a department (for Manager, only their assigned departments).
  - Lists users in that department; each row has “Xem công trình” to `WorkScoreDetailPage`.

- `WorkScoreDetailPage` (`/cham-diem/user/:userId`):
  - Shows basic user info.
  - Lists that user’s works in current academic year.
  - Uses `useWorkDialogs` in admin/manager mode:
    - `WorkUpdateDialog` for adjusting work/author info.
    - `WorkStatusDialog` for proof status.
    - `WorkNoteDialog` for notes.
  - “Xuất Excel” uses `exportWorksByAdmin(userId, academicYearId?, proofStatus?)`.

### Assignments & System Config

- `AssignmentPage` (`/phan-cong`):
  - Lists managers and their assigned departments.
  - `AssignmentForm` allows Admin to assign multiple departments per manager via dynamic form.
  - “Bỏ phân công” clears assignments for a manager.

- `SystemConfigPage` (`/cau-hinh-he-thong`):
  - Shows system configs for current academic year.
  - Provides actions:
    - Add/Edit config via `SystemConfigForm`.
    - Delete config.
    - Notify (locks editing/deletion).
  - Uses `isSystemOpen(config)` to determine global system open/closed and display status.

### Reporting & Statistics

- `ReportPage` (`/bao-cao`):
  - For current user (or manager) to filter their works:
    - Filter by academic year, proof status, source, and registration flags.
  - Fetches via `getWorksWithFilter(filter)`.
  - Exports via `exportWorks(filter)` (Excel).
  - Retrieves conversion summary via `getUserConversionResult(userId)` and displays aggregated hours.

- `StatisticsPage` (`/thong-ke`):
  - Manager/Admin-level stats:
    - Filters by academic year, department, user, proof status, source, and registration.
  - Fetches via `getWorksWithFilter({ ...filter, isCurrentUser: false })`.
  - Exports all works via `exportAllWorks(academicYearId?, proofStatus?, source?)`.
  - Imports works from Excel via `importExcel(file)`.

### Settings & Admin

- All settings pages share a pattern:
  - Load list via `useQuery`.
  - Show DataGrid via `DataTable`.
  - Provide Add/Edit dialogs powered by `react-hook-form` + `zod`.
  - Provide delete confirmation dialogs.

Examples:

- `AcademicYearPage` (`/cai-dat/nam-hoc`): manage academic years.
- `FactorPage` (`/cai-dat/he-so-quy-doi`): manage factors with score levels and convert hours.
- `UserPage` (`/quan-ly-tai-khoan`): manage users; includes Excel import for users.
- `CachePage` (`/cai-dat/quan-ly-cache`): list and clear backend caches.

### Utility/Test

- `TestPage` (`/test`):
  - A rich manual-testing UI with its own inline Axios client.
  - Useful for debugging but not part of normal user workflows.
  - For new features, prefer `apiClient` and React Query instead of this test client.

---

## UI Components & UX Patterns

- **Layout**
  - Use Toolpad `DashboardLayout` + `PageContainer` via `Layout.tsx`.
  - Top-right header uses `CustomAccount` with `AccountInfo` and `NotificationDropdown`.

- **Tables**
  - Prefer `DataTable` (MUI DataGrid wrapper) for interactive tables:
    - Use `columns: GridColDef[]` and `data` arrays.
    - Set widths and custom `renderCell` as needed.
  - Use `WorksCollapsibleTable` for detailed work lists (main + details rows).

- **Forms**
  - Use `react-hook-form` + `zod` for validation.
  - Wrap MUI inputs with `Controller` for non-simple fields.
  - Reuse `WorkForm` and `useWorkFormData` for work create/edit flows.

- **Dialogs**
  - Use MUI `Dialog` for add/edit/confirm actions.
  - Always disable buttons and show `CircularProgress` while mutations are in flight.
  - Close dialogs in `onSuccess` handlers and trigger invalidation/refetch.

- **Notifications & Errors**
  - Use `react-toastify`:
    - Success toasts on fetch/mutation success (with stable `toastId` for fetches).
    - Error toasts on `error` changes or mutation failures.
  - Use inline `CircularProgress` for loading states and `Alert`/`Typography` for visible error messages.

- **Time & Localization**
  - Use Luxon (`formatDateTime`) for date-time strings in Asia/Saigon, in Vietnamese.
  - Use date-fns (`formatMonthYear`, `format`) with `vi` locale for month/year and other date fields.
  - DataGrid uses Vietnamese locale via `viVN`.

---

## State Management & Context

- **AuthContext**
  - Only global client-side context used besides Toolpad session.
  - Provides `user`, `setUser`, `signOut`, `loading`, `refreshUserInfo`.
  - `AuthProvider` wraps the app in `main.tsx`.

- **React Query**
  - Handles all server-side data:
    - Queries for fetching lists and details.
    - Mutations for create/update/delete/export/import.
  - Query keys are descriptive and grouped by domain (`["works", ...]`, `["users", ...]`, `["system-configs"]`, etc.).

- **Local state**
  - Boolean flags for dialogs, selected items, filters, active steps (e.g., `step` in SignUp, `activeTab` in `WorkUpdateDialog`).
  - For optimistic UI (e.g., registration) a small local map (`optimisticUpdates`) is used.

---

## Conventions & Best Practices

When implementing new features or modifying existing ones:

- **Routing & roles**
  - Declare new routes in `Router.tsx`.
  - Wrap protected routes in `ProtectedRoute` with appropriate `allowedRoles`.
  - Keep `Navigation.tsx` in sync with new routes (for each role’s tree).

- **APIs**
  - Create new API wrappers in `src/lib/api/*Api.ts`.
  - Use `apiClient` with existing JSON and JWT patterns.
  - Return `ApiResponse<T>` to stay consistent.

- **React Query**
  - Always give queries stable, descriptive `queryKey`s.
  - Invalidate relevant keys in mutations (`onSuccess`) rather than manual state tweaks where possible.
  - For performance or responsiveness, you may also update local `data` after a mutation, but still invalidate the query.

- **Forms**
  - Use `react-hook-form` with `zodResolver` for major forms.
  - Keep error messages localized in Vietnamese.
  - For dynamic domain-bound fields (like work details), reuse existing utilities (`workDetailsConfig`, `scoreLevelUtils`).

- **System open/close logic**
  - For any operations tied to “open” windows (like editing/deleting works), use:
    - `useSystemStatus` (preferred) or
    - `systemCheck.isSystemOpen(config)` for config lists.
  - Respect `canEditWork`/`canDeleteWork` decisions rather than re-implementing proof status rules.

- **Excel flows**
  - Exports should call the appropriate function in `excelApi.ts` and trigger browser download.
  - Imports should:
    - Use hidden file inputs bound to buttons.
    - Show toasts on success/failure.
    - Invalidate the relevant queries (e.g., `["users"]`, `["works", ...]`).

- **Test/sandbox code**
  - `TestPage` is a sandbox and not a pattern to copy.
  - New production screens should not re-create local Axios clients; use `apiClient` and React Query instead.

---

## How to Use This Agent

With this spec, an LLM agent can:

- Answer **structural questions**:
  - “Where is the work registration logic implemented?”
  - “Which query keys are used for user-level vs department-level stats?”

- Guide **feature additions**:
  - “Add a new admin-only report under `/bao-cao` using the existing `DataTable` pattern.”
  - “Introduce a new field on works and wire it into `WorkForm`, `WorkUpdateDialog`, and the scoring views.”

- Suggest **data-flow changes**:
  - “Add a new filter to the statistics page and adapt `getWorksWithFilter` usage accordingly.”
  - “Extend the conversion summary to include a new category.”

- Help with **auth & role logic**:
  - “Restrict a page to Managers and Admins only.”
  - “Show/hide UI actions based on `user.role` and system open/close state.”

When asking the agent for help, mention:

- The page or route (e.g., `/cong-trinh`, `/bao-cao`, `/thong-ke`).
- The domain (works, users, assignments, settings, system config).
- Whether the change is for Users, Managers, or Admins.

The agent will then:

- Use the patterns documented here (React Query, AuthContext, API clients, MUI + Toolpad) to propose or implement changes that are consistent with the existing codebase.

