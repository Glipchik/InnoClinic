using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Documents.Application.Mapper
{
    public partial class ApplicationMapping : Profile
    {
        partial void AddResultMapping();
        partial void AddAppointmentMapping();
        partial void AddDoctorMapping();
        partial void AddPatientMapping();

        public ApplicationMapping()
        {
            AddResultMapping();
            AddAppointmentMapping();
            AddDoctorMapping();
            AddPatientMapping();
        }
    }
}
