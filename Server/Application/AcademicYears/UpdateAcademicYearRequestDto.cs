﻿namespace Application.AcademicYears
{
    public class UpdateAcademicYearRequestDto
    {
        public required string Name { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
    }
}
