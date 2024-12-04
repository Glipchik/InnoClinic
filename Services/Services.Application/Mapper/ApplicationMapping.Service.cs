using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Services.Application.Models;
using Services.Domain.Entities;

namespace Services.Application.Mapper
{
    partial class ApplicationMapping : Profile
    {
        partial void AddServiceMapping()
        {
            CreateMap<UpdateServiceModel, Service>();
            CreateMap<Service, ServiceModel>();
            CreateMap<CreateServiceModel, Service>();
        }
    }
}
