using Appointments.Domain.Entities;

namespace Appointments.API.DTOs
{
    public record CreateAppointmentDto(
        Guid DoctorId,
        Guid ServiceId,
        int TimeSlotId,
        DateOnly Date);

    public record UpdateAppointmentDto(
        Guid Id,
        int TimeSlotId,
        DateOnly Date);

    public record GetScheduleDto(
        Guid DoctorId,
        DateOnly Date);
}
