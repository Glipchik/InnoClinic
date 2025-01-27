using Appointments.API.DTOs;
using Appointments.Application.Models;

namespace Appointments.API.Mapper
{
    public partial class ApiMapping
    {
        partial void AddAppointmentMapping()
        {
            CreateMap<CreateAppointmentDto, CreateAppointmentModel>();
        }
    }
}
