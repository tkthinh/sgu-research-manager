namespace Application.WorkTypes
{
   public class WorkTypeWithDetailsCountDto
    {
      public Guid Id { get; set; }
      public required string Name { get; set; }
      public int? WorkLevelCount { get; set; }
      public int? PurposeCount { get; set; }
      public int? AuthorRoleCount { get; set; }
      public int? FactorCount { get; set; }
      public int? SCImagoFieldCount { get; set; }
      public ICollection<WorkLevelInfo>? WorkLevels { get; set; }
      public ICollection<PurposeInfo>? Purposes { get; set; }
      public ICollection<AuthorRoleInfo>? AuthorRoles { get; set; }
      public ICollection<SCImagoFieldInfo>? SCImagoFields { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime? ModifiedDate { get; set; }
   }

   public class WorkLevelInfo
   {
      public Guid Id { get; set; }
      public required string Name { get; set; }
   }

   public class PurposeInfo
   {
      public Guid Id { get; set; }
      public required string Name { get; set; }
   }

   public class AuthorRoleInfo
   {
      public Guid Id { get; set; }
      public required string Name { get; set; }
   }

   public class SCImagoFieldInfo
   {
      public Guid Id { get; set; }
      public required string Name { get; set; }
   }
}
