using AutoMapper;
using Events.Doctor;
using Events.Office;
using Offices.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Consumers.Mapper
{
    public partial class ConsumersMapping : Profile
    {
        partial void AddDoctorMapping()
        {
            CreateMap<DoctorCreated, Doctor>();
            CreateMap<DoctorUpdated, Doctor>();
        }
    }
}
