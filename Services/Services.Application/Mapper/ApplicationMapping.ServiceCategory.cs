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
        partial void AddServiceCategoryMapping()
        {
            CreateMap<UpdateServiceCategoryModel, ServiceCategory>();
            CreateMap<ServiceCategory, ServiceCategoryModel>();
            CreateMap<CreateServiceCategoryModel, ServiceCategory>();
        }
    }
}
