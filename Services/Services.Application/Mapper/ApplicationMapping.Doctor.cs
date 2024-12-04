using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Application.Models;
using Services.Domain.Entities;

namespace Services.Application.Mapper
{
    public partial class ApplicationMapping
    {
        partial void AddDoctorMapping()
        {
            CreateMap<UpdateDoctorModel, Doctor>();
            CreateMap<Doctor, DoctorModel>();
            CreateMap<CreateDoctorModel, Doctor>();
        }
    }
}
