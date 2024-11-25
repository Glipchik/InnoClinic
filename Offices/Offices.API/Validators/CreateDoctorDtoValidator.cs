﻿using FluentValidation;
using MongoDB.Bson;
using Offices.API.DTOs;

namespace Offices.API.Validators
{
    public class CreateDoctorDtoValidator : AbstractValidator<CreateDoctorDto>
    {
        public CreateDoctorDtoValidator()
        {
            RuleFor(doctor => doctor.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(doctor => doctor.MiddleName).MaximumLength(50);
            RuleFor(doctor => doctor.LastName).NotEmpty().MaximumLength(50);
            RuleFor(doctor => doctor.OfficeId).NotEmpty().WithMessage("Id must not be empty")
                .Must(id => ObjectId.TryParse(id, out _))
                .WithMessage("Invalid ObjectId format");
            RuleFor(doctor => doctor.Status).NotEmpty().MaximumLength(20)
                .Must(status => AllowedStatuses.Contains(status))
                .WithMessage("Status must be one of the following: " + string.Join(", ", AllowedStatuses));
        }

        private static readonly string[] AllowedStatuses = { "At work", "On vacation", "Sick Day", "Sick Leave", "Self-isolation", "Leave without pay", "Inactive" };
    }
}
