using FluentValidation;
using Profiles.API.DTO.Enums;
using Profiles.API.DTOs;
using System;

namespace Profiles.API.Validators;

public class CreateDoctorDtoValidator : AbstractValidator<CreateDoctorDto>
{
    public CreateDoctorDtoValidator()
    {
        RuleFor(doctor => doctor.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(doctor => doctor.MiddleName).MaximumLength(50);
        RuleFor(doctor => doctor.LastName).NotEmpty().MaximumLength(50);
        RuleFor(doctor => doctor.SpecializationId).NotEmpty().WithMessage("Specialization id must not be empty");
        RuleFor(doctor => doctor.OfficeId).NotEmpty().WithMessage("Specialization id must not be empty");
        RuleFor(patient => patient.DateOfBirth)
                        .NotEmpty().WithMessage("Date of birth must not be empty")
                        .Must(BeAValidDate).WithMessage("Date of birth must be a valid date")
                        .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future")
                        .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Date of birth cannot be more than 120 years ago");
        RuleFor(doctor => doctor.CareerStartYear).NotEmpty().WithMessage("Career start year must not be empty");
        RuleFor(doctor => doctor.Status).Must(IsValidEnumValue).WithMessage("Invalid doctor status value.");
    }

    private bool IsValidEnumValue(DoctorStatusDto status)
    {
        return Enum.IsDefined(typeof(DoctorStatusDto), status);
    }
    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}

public class UpdateDoctorDtoValidator : AbstractValidator<UpdateDoctorDto>
{
    public UpdateDoctorDtoValidator()
    {
        RuleFor(doctor => doctor.Id).NotEmpty().WithMessage("Id must not be empty");
        RuleFor(doctor => doctor.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(doctor => doctor.MiddleName).MaximumLength(50);
        RuleFor(doctor => doctor.LastName).NotEmpty().MaximumLength(50);
        RuleFor(patient => patient.DateOfBirth)
                        .NotEmpty().WithMessage("Date of birth must not be empty")
                        .Must(BeAValidDate).WithMessage("Date of birth must be a valid date")
                        .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future")
                        .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Date of birth cannot be more than 120 years ago");
        RuleFor(doctor => doctor.CareerStartYear).NotEmpty().WithMessage("Career start year must not be empty");
    }

    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}

public class UpdateDoctorByReceptionistDtoValidator : AbstractValidator<UpdateDoctorByReceptionistDto>
{
    public UpdateDoctorByReceptionistDtoValidator()
    {
        RuleFor(doctor => doctor.Id).NotEmpty().WithMessage("Id must not be empty");
        RuleFor(doctor => doctor.FirstName).NotEmpty().MaximumLength(50);
        RuleFor(doctor => doctor.MiddleName).MaximumLength(50);
        RuleFor(doctor => doctor.LastName).NotEmpty().MaximumLength(50);
        RuleFor(patient => patient.DateOfBirth)
                        .NotEmpty().WithMessage("Date of birth must not be empty")
                        .Must(BeAValidDate).WithMessage("Date of birth must be a valid date")
                        .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future")
                        .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Date of birth cannot be more than 120 years ago");
        RuleFor(doctor => doctor.CareerStartYear).NotEmpty().WithMessage("Career start year must not be empty");
    }

    private bool BeAValidDate(DateTime date)
    {
        return !date.Equals(default(DateTime));
    }
}