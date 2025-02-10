using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Profiles.Application.Models;
using Profiles.Application.Models.Enums;
using Profiles.Domain.Entities;
using Profiles.Domain.Enums;

namespace Profiles.Application.Mapper
{
    public partial class ApplicationMapping
    {
        partial void AddDoctorMapping()
        {
            CreateMap<UpdateDoctorModel, Doctor>();
            CreateMap<Doctor, DoctorModel>();
            CreateMap<CreateDoctorModel, Doctor>();
            CreateMap<DoctorStatusModel, DoctorStatus>();
        }
    }
}
