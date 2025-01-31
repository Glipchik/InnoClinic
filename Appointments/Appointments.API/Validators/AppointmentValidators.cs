using Appointments.API.DTOs;
using FluentValidation;

namespace Appointments.API.Validators
{
    public class CreateAppointmentDtoValidator : AbstractValidator<CreateAppointmentDto>
    {
        public CreateAppointmentDtoValidator()
        {
            RuleFor(appointment => appointment.DoctorId).NotEmpty();
            RuleFor(appointment => appointment.ServiceId).NotEmpty();
            RuleFor(appointment => appointment.TimeSlotId).NotEmpty().GreaterThan(0).WithMessage("Time slot id must be more than 0");
            RuleFor(appointment => appointment.Date).NotEmpty().WithMessage("Date of appointment must not be empty");
        }
    }

    public class UpdateAppointmentDtoValidator : AbstractValidator<UpdateAppointmentDto>
    {
        public UpdateAppointmentDtoValidator()
        {
            RuleFor(appointment => appointment.Id).NotEmpty();
            RuleFor(appointment => appointment.TimeSlotId).NotEmpty().GreaterThan(0).WithMessage("Time slot id must be more than 0");
            RuleFor(appointment => appointment.Date).NotEmpty().WithMessage("Date of appointment must not be empty");
        }
    }

    public class GetScheduleDtoValidator : AbstractValidator<GetScheduleDto>
    {
        public GetScheduleDtoValidator()
        {
            RuleFor(appointment => appointment.DoctorId).NotEmpty();
            RuleFor(appointment => appointment.Date).NotEmpty().WithMessage("Date of appointment must not be empty");
        }
    }
}
