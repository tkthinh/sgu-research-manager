﻿using System;
using System.Collections.Generic;

namespace Application.Purposes
{
   public class PurposeDto
   {
      public Guid Id { get; set; }
      public required string Name { get; set; }
      public Guid WorkTypeId { get; set; }
      public string? WorkTypeName { get; set; }
      public DateTime CreatedDate { get; set; }
      public DateTime? ModifiedDate { get; set; }
   }
}
