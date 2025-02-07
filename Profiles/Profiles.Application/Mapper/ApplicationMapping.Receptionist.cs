using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Profiles.Application.Models;
using Profiles.Domain.Entities;

namespace Profiles.Application.Mapper
{
    partial class ApplicationMapping : Profile
    {
        partial void AddReceptionistMapping()
        {
            CreateMap<UpdateReceptionistModel, Receptionist>();
            CreateMap<Receptionist, ReceptionistModel>();
            CreateMap<CreateReceptionistModel, Receptionist>();
        }
    }
}
