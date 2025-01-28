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

        public ApplicationMapping()
        {
            AddResultMapping();
            AddAppointmentMapping();
        }
    }
}
