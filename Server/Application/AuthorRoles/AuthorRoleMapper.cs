using Domain.Entities;
using Domain.Interfaces;

namespace Application.AuthorRoles
{
    public class AuthorRoleMapper : IGenericMapper<AuthorRoleDto, AuthorRole>
    {
        public AuthorRoleDto MapToDto(AuthorRole entity)
        {
            return new AuthorRoleDto
            {
                Id = entity.Id,
                Name = entity.Name,
                IsMainAuthor = entity.IsMainAuthor,
                WorkTypeId = entity.WorkTypeId,
                WorkTypeName = entity.WorkType?.Name,
                CreatedDate = entity.CreatedDate,
                ModifiedDate = entity.ModifiedDate
            };
        }

        public AuthorRole MapToEntity(AuthorRoleDto dto)
        {
            return new AuthorRole
            {
                Id = dto.Id,
                Name = dto.Name,
                IsMainAuthor = dto.IsMainAuthor,
                WorkTypeId = dto.WorkTypeId,
                CreatedDate = dto.CreatedDate,
                ModifiedDate = dto.ModifiedDate
            };
        }

        public IEnumerable<AuthorRoleDto> MapToDtos(IEnumerable<AuthorRole> entities)
        {
            return entities.Select(MapToDto);
        }

        public IEnumerable<AuthorRole> MapToEntities(IEnumerable<AuthorRoleDto> dtos)
        {
            return dtos.Select(MapToEntity);
        }
    }
}
