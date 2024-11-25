using FluentValidation;
using MongoDB.Bson;
using Offices.API.DTOs;

namespace Offices.API.Validators
{
    public class UpdateOfficeDtoValidator : AbstractValidator<UpdateOfficeDto>
    {
        public UpdateOfficeDtoValidator()
        {
            RuleFor(office => office.Id)
                .NotEmpty().WithMessage("Id must not be empty")
                .Must(id => ObjectId.TryParse(id, out _))
                .WithMessage("Invalid ObjectId format");
            RuleFor(office => office.Address).NotEmpty().MaximumLength(150);
            RuleFor(office => office.RegistryPhoneNumber).NotEmpty().MaximumLength(20);
        }
    }
}
