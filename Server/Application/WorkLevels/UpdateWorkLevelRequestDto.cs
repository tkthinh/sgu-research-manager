﻿namespace Application.WorkLevels
{
    public class UpdateWorkLevelRequestDto
    {
        public required string Name { get; set; }
        public Guid WorkTypeId { get; set; }
    }
}
