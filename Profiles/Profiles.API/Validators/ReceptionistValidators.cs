using FluentValidation;
using Profiles.API.DTO.Enums;
using Profiles.API.DTOs;

namespace Profiles.API.Validators
{
    public class CreateReceptionistDtoValidator : AbstractValidator<CreateReceptionistDto>
    {
        public CreateReceptionistDtoValidator()
        {
            RuleFor(receptionist => receptionist.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(receptionist => receptionist.MiddleName).MaximumLength(50);
            RuleFor(receptionist => receptionist.LastName).NotEmpty().MaximumLength(50);
            RuleFor(receptionist => receptionist.OfficeId).NotEmpty().WithMessage("Specialization id must not be empty");
        }
    }

    public class UpdateReceptionistDtoValidator : AbstractValidator<UpdateReceptionistDto>
    {
        public UpdateReceptionistDtoValidator()
        {
            RuleFor(receptionist => receptionist.Id).NotEmpty().WithMessage("Id must not be empty");
            RuleFor(receptionist => receptionist.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(receptionist => receptionist.MiddleName).MaximumLength(50);
            RuleFor(receptionist => receptionist.LastName).NotEmpty().MaximumLength(50);
        }
    }
}
