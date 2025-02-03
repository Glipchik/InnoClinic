using Documents.Application.Models;
using Documents.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents.Application.Mapper
{
    public partial class ApplicationMapping
    {
        partial void AddAppointmentMapping()
        {
            CreateMap<Appointment, AppointmentModel>();
        }
    }
}
