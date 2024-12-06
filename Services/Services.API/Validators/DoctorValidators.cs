using FluentValidation;
using Services.API.DTOs;
using Services.API.DTOs.Enums;
using Services.Domain.Entities;

namespace Services.API.Validators
{
    public class CreateDoctorDtoValidator : AbstractValidator<CreateDoctorDto>
    {
        public CreateDoctorDtoValidator()
        {
            RuleFor(doctor => doctor.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(doctor => doctor.MiddleName).MaximumLength(50);
            RuleFor(doctor => doctor.LastName).NotEmpty().MaximumLength(50);
            RuleFor(doctor => doctor.SpecializationId).NotEmpty().WithMessage("Specialization id must not be empty");
            RuleFor(doctor => doctor.Status).Must(IsValidEnumValue).WithMessage("Invalid doctor status value.");
        }

        private bool IsValidEnumValue(DoctorStatusDto status)
        {
            return Enum.IsDefined(typeof(DoctorStatusDto), status);
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
            RuleFor(doctor => doctor.SpecializationId).NotEmpty().WithMessage("Specialization id must not be empty");
            RuleFor(doctor => doctor.Status).Must(IsValidEnumValue).WithMessage("Invalid doctor status value.");
        }

        private bool IsValidEnumValue(DoctorStatusDto status)
        {
            return Enum.IsDefined(typeof(DoctorStatusDto), status);
        }
    }
}
