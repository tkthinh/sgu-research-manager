using Application.Shared.Response;
using Application.Shared.Services;
using Application.Users;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Application.Assignments
{
   public class AssignmentService : GenericCachedService<AssignmentDto, Assignment>, IAssignmentService
   {
      private readonly IAssignmentRepository repository;
      public AssignmentService(
          IUnitOfWork unitOfWork,
          IAssignmentRepository repository,
          IGenericMapper<AssignmentDto, Assignment> mapper,
          IDistributedCache cache,
          ILogger<AssignmentService> logger
      ) : base(unitOfWork, mapper, cache, logger)
      {
         this.repository = repository;
      }

      public override async Task<IEnumerable<AssignmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
      {
         // Retrieve assignments with detailed information (e.g., including user and department info)
         var assignments = await repository.GetAssignmentsWithDetailsAsync(cancellationToken);
         return mapper.MapToDtos(assignments);
      }

      public override async Task<AssignmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
      {
         // Retrieve the assignment by ID
         var assignment = await unitOfWork.Repository<Assignment>().GetByIdAsync(id);
         if (assignment == null)
         {
            throw new Exception("Assignment not found");
         }

         // Map the assignment entity to a DTO
         var assignmentDto = mapper.MapToDto(assignment);
         return assignmentDto;
      }

      public async Task<List<AssignmentDto>> CreateAsync(CreateAssignmentRequestDto request, CancellationToken cancellationToken = default)
      {
         // Validate the user existence
         var manager = await unitOfWork.Repository<User>().GetByIdAsync(request.ManagerId);
         if (manager is null)
         {
            throw new Exception("Manager not found");
         }

         var result = new List<AssignmentDto>();

         foreach (var deptId in request.DepartmentIds)
         {
            // Validate the department exists before assignment
            var department = await unitOfWork.Repository<Department>().GetByIdAsync(deptId);
            if (department is null)
            {
               // Optionally, you can choose to throw an error or skip this department
               continue;
            }

            // Check if the assignment already exists to avoid duplicates
            var exists = await unitOfWork.Repository<Assignment>().AnyAsync(a => a.UserId == request.ManagerId && a.DepartmentId == deptId);
            if (!exists)
            {
               var assignment = new Assignment
               {
                  User = manager,
                  UserId = request.ManagerId,
                  DepartmentId = deptId,
               };

               await unitOfWork.Repository<Assignment>().CreateAsync(assignment);
               result.Add(mapper.MapToDto(assignment));
            }
         }

         await unitOfWork.SaveChangesAsync();
         await SafeInvalidateCacheAsync();
         return result;
      }

      public async Task<List<AssignmentDto>> UpdateAsync(UpdateAssignmentRequestDto request, CancellationToken cancellationToken = default)
      {
         // Validate the user existence
         var manager = await unitOfWork.Repository<User>().GetByIdAsync(request.ManagerId);
         if (manager is null)
            throw new Exception("Manager not found");

         var assignmentRepository = unitOfWork.Repository<Assignment>();

         // Retrieve existing assignments for the user
         var existingAssignments = await assignmentRepository.FindAsync(a => a.UserId == request.ManagerId);
         var currentDeptIds = existingAssignments.Select(a => a.DepartmentId).ToList();
         var newDeptIds = request.DepartmentIds;

         // Calculate which assignments need to be removed and which to add
         var toRemove = existingAssignments.Where(a => !newDeptIds.Contains(a.DepartmentId)).ToList();
         var toAdd = newDeptIds.Except(currentDeptIds);

         // Remove outdated assignments
         foreach (var assignment in toRemove)
         {
            await assignmentRepository.DeleteAsync(assignment.Id);
         }

         // Add new assignments
         var result = new List<AssignmentDto>();
         foreach (var deptId in toAdd)
         {
            // Validate the department exists
            var department = await unitOfWork.Repository<Department>().GetByIdAsync(deptId);
            if (department is null)
            {
               continue;
            }

            var newAssignment = new Assignment
            {
               User = manager,
               UserId = request.ManagerId,
               DepartmentId = deptId,
            };

            await assignmentRepository.CreateAsync(newAssignment);
            result.Add(mapper.MapToDto(newAssignment));
         }

         await unitOfWork.SaveChangesAsync();
         await SafeInvalidateCacheAsync();
         return result;
      }

      public async Task<List<AssignmentDto>> GetAllAssignmentByUserAsync(Guid userId, CancellationToken cancellationToken = default)
      {
         // Retrieve all assignments for the given user
         var assignments = await unitOfWork.Repository<Assignment>().FindAsync(a => a.UserId == userId);

         // Map the assignments to DTOs
         var assignmentDtos = mapper.MapToDtos(assignments);

         return assignmentDtos.ToList();
      }


      public async Task DeleteAllAssignmentsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
      {
         // Retrieve all assignments for the given user
         var assignments = await unitOfWork.Repository<Assignment>().FindAsync(a => a.UserId == userId);

         // Delete each assignment
         foreach (var assignment in assignments)
         {
            await unitOfWork.Repository<Assignment>().DeleteAsync(assignment.Id);
         }

         // Commit the changes
         await unitOfWork.SaveChangesAsync(cancellationToken);
      }
   }
}
