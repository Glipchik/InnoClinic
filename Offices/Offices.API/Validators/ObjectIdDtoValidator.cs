using FluentValidation;
using MongoDB.Bson;
using Offices.API.DTOs;

namespace Offices.API.Validators
{
    public class ObjectIdDtoValidator : AbstractValidator<ObjectIdDto>
    {
        public ObjectIdDtoValidator() 
        {
            RuleFor(i => i.Id)
                .NotEmpty().WithMessage("Id must not be empty")
                .Must(id => ObjectId.TryParse(id, out _))
                .WithMessage("Invalid ObjectId format");
        }
    }
}
