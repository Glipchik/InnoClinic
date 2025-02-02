using FluentValidation;
using Profiles.API.DTOs;

namespace Profiles.API.Validators
{
    public class CreateAccountDtoValidator : AbstractValidator<CreateAccountDto>
    {
        public CreateAccountDtoValidator()
        {
            RuleFor(account => account.Email).NotEmpty().WithMessage("Id must not be empty").EmailAddress().WithMessage("Invalid email format");
            RuleFor(account => account.PhoneNumber).NotEmpty().MaximumLength(20).WithMessage("Invalid phone format");
        }
    }

    public class CreateAccountFromAuthDtoValidator : AbstractValidator<CreateAccountFromAuthDto>
    {
        public CreateAccountFromAuthDtoValidator()
        {
            RuleFor(account => account.Email).NotEmpty().WithMessage("Id must not be empty").EmailAddress().WithMessage("Invalid email format");
            RuleFor(account => account.PhoneNumber).NotEmpty().MaximumLength(20).WithMessage("Invalid phone format");
        }
    }
}
