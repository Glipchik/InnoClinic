using FluentValidation;
using Services.API.DTOs;

namespace Services.API.Validators
{
    public class CreateServiceCategoryDtoValidator : AbstractValidator<CreateServiceCategoryDto>
    {
        public CreateServiceCategoryDtoValidator()
        {
            RuleFor(sc => sc.CategoryName).NotEmpty().MaximumLength(50);
        }
    }

    public class UpdateServiceCategoryDtoValidator : AbstractValidator<UpdateServiceCategoryDto>
    {
        public UpdateServiceCategoryDtoValidator()
        {
            RuleFor(sc => sc.Id).NotEmpty().WithMessage("Id must not be empty");
            RuleFor(sc => sc.CategoryName).NotEmpty().MaximumLength(50);
        }
    }
}
