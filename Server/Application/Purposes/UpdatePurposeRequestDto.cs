﻿namespace Application.Purposes
{
   public class UpdatePurposeRequestDto
   {
      public required string Name { get; set; }
      public Guid WorkTypeId { get; set; }
    }
}
