using Appointments.Application.Models;
using Appointments.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Application.Mapper
{
    public partial class ApplicationMapping
    {
        partial void AddPatientMapping()
        {
            CreateMap<Patient, PatientModel>();
        }
    }
}
