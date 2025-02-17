﻿using Profiles.Application.Models.Enums;
using Profiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Models
{
    public class CreateDoctorModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public Guid SpecializationId { get; set; }
        public DateTime CareerStartYear { get; set; }
        public Guid OfficeId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DoctorStatusModel Status { get; set; }
    }

    public class DoctorModel
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public required SpecializationModel Specialization { get; set; }
        public required AccountModel Account { get; set; }
        public required OfficeModel Office { get; set; }
        public DateTime CareerStartYear { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DoctorStatusModel Status { get; set; }
    }

    public class UpdateDoctorModel
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public Guid SpecializationId { get; set; }
        public DateTime CareerStartYear { get; set; }
        public Guid OfficeId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DoctorStatusModel Status { get; set; }
        public Guid AuthorId { get; set; }
    }

    public record DoctorQueryParametresModel(
        Guid? OfficeId,
        Guid? SpecializationId,
        DoctorStatusModel? Status);
}
