﻿using Domain.Entities;

namespace Application.AuthorRegistrations
{
    public class AuthorRegistrationDto
    {
        public Guid Id { get; set; }
        public Guid AcademicYearId { get; set; }
        public Guid AuthorId { get; set; }

        public string? AcademicYearName { get; set; }
    }
}
