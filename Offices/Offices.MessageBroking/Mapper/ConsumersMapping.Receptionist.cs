using AutoMapper;
using Events.Doctor;
using Events.Receptionist;
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
        partial void AddReceptionistMapping()
        {
            CreateMap<ReceptionistCreated, Receptionist>();
            CreateMap<ReceptionistUpdated, Receptionist>();
        }
    }
}
