using FluentValidation;

namespace Authorization.Presentation.Validators
{
    public class CreateAccountInputModelValidator : AbstractValidator<Pages.Create.InputModel>
    {
        public CreateAccountInputModelValidator()
        {
            RuleFor(patient => patient.FirstName).NotEmpty().MaximumLength(50);

            RuleFor(patient => patient.MiddleName).MaximumLength(50);

            RuleFor(patient => patient.LastName).NotEmpty().MaximumLength(50);

            RuleFor(patient => patient.DateOfBirth)
                            .NotEmpty().WithMessage("Date of birth must not be empty")
                            .Must(BeAValidDate).WithMessage("Date of birth must be a valid date")
                            .LessThan(DateTime.Now).WithMessage("Date of birth cannot be in the future")
                            .GreaterThan(DateTime.Now.AddYears(-120)).WithMessage("Date of birth cannot be more than 120 years ago");

            RuleFor(acc => acc.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(100).WithMessage("Email is too long")
                .EmailAddress().WithMessage("Invalid format of email address");
            RuleFor(acc => acc.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .MaximumLength(50).WithMessage("Email is too long");
            RuleFor(acc => acc.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches("^[0-9\\-\\+]{9,15}$").WithMessage("Invalid format of phone number");
        }

        private bool BeAValidDate(DateTime? date)
        {
            return !date.Equals(default(DateTime));
        }
    }

    public class CreateAccountDtoValidator : AbstractValidator<DTOs.CreateAccountDto>
    {
        public CreateAccountDtoValidator() 
        {
            RuleFor(acc => acc.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(50).WithMessage("Email is too long")
                .EmailAddress().WithMessage("Invalid format of email address");
            RuleFor(acc => acc.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required")
                .Matches("^[0-9\\-\\+]{9,15}$").WithMessage("Invalid format of phone number");
        }
    }

    public class LoginAccountInputModelValidator : AbstractValidator<Pages.Login.InputModel>
    {
        public LoginAccountInputModelValidator()
        {
            RuleFor(acc => acc.Email)
                .NotEmpty().WithMessage("Email is required")
                .MaximumLength(50).WithMessage("Email is too long")
                .EmailAddress().WithMessage("Invalid format of email address");
            RuleFor(acc => acc.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
                .MaximumLength(50).WithMessage("Email is too long");
        }
    }
}