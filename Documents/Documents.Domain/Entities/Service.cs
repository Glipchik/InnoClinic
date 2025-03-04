﻿namespace Documents.Domain.Entities
{
    public class Service : BaseEntity
    {
        public required string ServiceName { get; set; }
        public required Guid SpecializationId { get; set; }
        public required Specialization Specialization { get; set; }
        public required Guid ServiceCategoryId { get; set; }
        public required ServiceCategory ServiceCategory { get; set; }
        public required decimal Price { get; set; }
        public required bool IsActive { get; set; } = true;
    }
}