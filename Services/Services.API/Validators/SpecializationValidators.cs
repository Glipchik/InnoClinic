using FluentValidation;
using Services.API.DTOs;

namespace Services.API.Validators
{
    public class CreateSpecializationDtoValidator : AbstractValidator<CreateSpecializationDto>
    {
        public CreateSpecializationDtoValidator()
        {
            RuleFor(specialization => specialization.SpecializationName).NotEmpty().MaximumLength(50);
        }
    }

    public class UpdateSpecializationDtoValidator : AbstractValidator<UpdateSpecializationDto>
    {
        public UpdateSpecializationDtoValidator()
        {
            RuleFor(specialization => specialization.Id).NotEmpty().WithMessage("Id must not be empty");
            RuleFor(specialization => specialization.SpecializationName).NotEmpty().MaximumLength(50);
        }
    }
}
