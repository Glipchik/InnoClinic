using FluentValidation;
using Offices.API.DTOs;

namespace Offices.API.Validators
{
    public class CreateOfficeDtoValidator: AbstractValidator<CreateOfficeDto>
    {
        public CreateOfficeDtoValidator()
        {
            RuleFor(office => office.Address).NotEmpty().MaximumLength(150);
            RuleFor(office => office.RegistryPhoneNumber).NotEmpty().MaximumLength(20);
        }
    }
}
