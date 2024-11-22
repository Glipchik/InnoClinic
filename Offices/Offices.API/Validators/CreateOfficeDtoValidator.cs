using FluentValidation;
using Offices.API.DTOs;

namespace Offices.API.Validators
{
    public class CreateOfficeDtoValidator: AbstractValidator<CreateOfficeDto>
    {
        public CreateOfficeDtoValidator()
        {
            RuleFor(office => office.Address).NotEmpty();
            RuleFor(office => office.RegistryPhoneNumber).NotEmpty();
        }
    }
}
