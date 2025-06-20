﻿using System;
using System.Collections.Generic;

namespace Application.SCImagoFields
{
    public class SCImagoFieldDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
