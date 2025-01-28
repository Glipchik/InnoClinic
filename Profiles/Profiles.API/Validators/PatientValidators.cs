using FluentValidation;
using Profiles.API.DTO.Enums;
using Profiles.API.DTOs;

namespace Profiles.API.Validators
{
    public class CreatePatientDtoValidator : AbstractValidator<CreatePatientDto>
    {
        public CreatePatientDtoValidator()
        {
            RuleFor(patient => patient.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(patient => patient.MiddleName).MaximumLength(50);
            RuleFor(patient => patient.LastName).NotEmpty().MaximumLength(50);
            RuleFor(patient => patient.DateOfBirth)
                            .NotEmpty().WithMessage("Date of birth must not be empty")
                            .Must(BeAValidDate).WithMessage("Date of birth must be a valid date")
                            .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future")
                            .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Date of birth cannot be more than 120 years ago");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }

    public class CreatePatientFromAuthServerDtoValidator : AbstractValidator<CreatePatientFromAuthServerDto>
    {
        public CreatePatientFromAuthServerDtoValidator()
        {
            RuleFor(patient => patient.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(patient => patient.MiddleName).MaximumLength(50);
            RuleFor(patient => patient.LastName).NotEmpty().MaximumLength(50);
            RuleFor(patient => patient.DateOfBirth)
                            .NotEmpty().WithMessage("Date of birth must not be empty")
                            .Must(BeAValidDate).WithMessage("Date of birth must be a valid date")
                            .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future")
                            .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Date of birth cannot be more than 120 years ago");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }

    public class UpdatePatientDtoValidator : AbstractValidator<UpdatePatientDto>
    {
        public UpdatePatientDtoValidator()
        {
            RuleFor(patient => patient.Id).NotEmpty().WithMessage("Id must not be empty");
            RuleFor(patient => patient.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(patient => patient.MiddleName).MaximumLength(50);
            RuleFor(patient => patient.LastName).NotEmpty().MaximumLength(50);
            RuleFor(patient => patient.DateOfBirth)
                            .NotEmpty().WithMessage("Date of birth must not be empty")
                            .Must(BeAValidDate).WithMessage("Date of birth must be a valid date")
                            .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future")
                            .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Date of birth cannot be more than 120 years ago");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }

    public class UpdatePatientByReceptionistDtoValidator : AbstractValidator<UpdatePatientByReceptionistDto>
    {
        public UpdatePatientByReceptionistDtoValidator()
        {
            RuleFor(patient => patient.Id).NotEmpty().WithMessage("Id must not be empty");
            RuleFor(patient => patient.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(patient => patient.MiddleName).MaximumLength(50);
            RuleFor(patient => patient.LastName).NotEmpty().MaximumLength(50);
            RuleFor(patient => patient.DateOfBirth)
                            .NotEmpty().WithMessage("Date of birth must not be empty")
                            .Must(BeAValidDate).WithMessage("Date of birth must be a valid date")
                            .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future")
                            .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Date of birth cannot be more than 120 years ago");
        }

        private bool BeAValidDate(DateTime date)
        {
            return !date.Equals(default(DateTime));
        }
    }
}
