using FluentValidation;
using MongoDB.Bson;
using Offices.API.DTOs;

namespace Offices.API.Validators
{
    public class CreateReceptionistDtoValidator: AbstractValidator<CreateReceptionistDto>
    {
        public CreateReceptionistDtoValidator()
        {
            RuleFor(receptionist => receptionist.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(receptionist => receptionist.MiddleName).MaximumLength(50);
            RuleFor(receptionist => receptionist.LastName).NotEmpty().MaximumLength(50);
            RuleFor(receptionist => receptionist.OfficeId).NotEmpty().WithMessage("Id must not be empty")
                .Must(id => ObjectId.TryParse(id, out _))
                .WithMessage("Invalid ObjectId format");
        }
    }
}
