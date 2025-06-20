﻿namespace Domain.Entities
{
    public class WorkType : BaseEntity
    {
        public required string Name { get; set; }

        public ICollection<AuthorRole>? AuthorRoles { get; set; }
        public ICollection<Factor>? Factors { get; set; }
        public ICollection<WorkLevel>? WorkLevels { get; set; }
        public ICollection<Purpose>? Purposes { get; set; }
        public ICollection<SCImagoField>? SCImagoFields { get; set; }
    }
}
