using Documents.API.DTOs;
using FluentValidation;

namespace Documents.API.Validators
{
    public class CreateResultDtoValidator : AbstractValidator<CreateResultDto>
    {
        public CreateResultDtoValidator()
        {
            RuleFor(appointment => appointment.AppointmentId).NotEmpty();

            RuleFor(appointment => appointment.Conclusion)
                .NotEmpty().WithMessage("Can't be empty")
                .MaximumLength(500).WithMessage("Too long");

            RuleFor(appointment => appointment.Conclusion)
                .NotEmpty().WithMessage("Can't be empty")
                .MaximumLength(500).WithMessage("Too long");

            RuleFor(appointment => appointment.Conclusion)
                .NotEmpty().WithMessage("Can't be empty")
                .MaximumLength(500).WithMessage("Too long");
        }
    }

    public class UpdateResultDtoValidator : AbstractValidator<UpdateResultDto>
    {
        public UpdateResultDtoValidator()
        {
            RuleFor(appointment => appointment.Id).NotEmpty();

            RuleFor(appointment => appointment.Conclusion)
                .NotEmpty().WithMessage("Can't be empty")
                .MaximumLength(500).WithMessage("Too long");

            RuleFor(appointment => appointment.Conclusion)
                .NotEmpty().WithMessage("Can't be empty")
                .MaximumLength(500).WithMessage("Too long");

            RuleFor(appointment => appointment.Conclusion)
                .NotEmpty().WithMessage("Can't be empty")
                .MaximumLength(500).WithMessage("Too long");
        }
    }
}
