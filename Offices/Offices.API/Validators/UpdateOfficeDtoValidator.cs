using FluentValidation;
using MongoDB.Bson;
using Offices.API.DTOs;

namespace Offices.API.Validators
{
    public class UpdateOfficeDtoValidator : AbstractValidator<UpdateOfficeDto>
    {
        public UpdateOfficeDtoValidator()
        {
            RuleFor(office => office.Address).NotEmpty().MaximumLength(150);
            RuleFor(office => office.RegistryPhoneNumber).NotEmpty().MaximumLength(20);
        }
    }
}
