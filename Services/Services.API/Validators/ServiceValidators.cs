using FluentValidation;
using Services.API.DTOs.Enums;
using Services.API.DTOs;

namespace Services.API.Validators
{
    public class CreateServiceDtoValidator : AbstractValidator<CreateServiceDto>
    {
        public CreateServiceDtoValidator()
        {
            RuleFor(service => service.ServiceName).NotEmpty().MaximumLength(50);
            RuleFor(service => service.SpecializationId).NotEmpty().WithMessage("Specialization id must not be empty");
            RuleFor(service => service.ServiceCategoryId).NotEmpty().WithMessage("Service id must not be empty");
        }
    }

    public class UpdateServiceDtoValidator : AbstractValidator<UpdateServiceDto>
    {
        public UpdateServiceDtoValidator()
        {
            RuleFor(service => service.Id).NotEmpty().WithMessage("Id must not be empty");
            RuleFor(service => service.ServiceName).NotEmpty().MaximumLength(50);
            RuleFor(service => service.SpecializationId).NotEmpty().WithMessage("Specialization id must not be empty");
            RuleFor(service => service.ServiceCategoryId).NotEmpty().WithMessage("Service id must not be empty");
        }
    }
}
