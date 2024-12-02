using AutoMapper;
using Services.Application.Models;
using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Mapper
{
    public class ApplicationMapping : Profile
    {
        public ApplicationMapping()
        {
            // Doctor mapping
            CreateMap<UpdateDoctorModel, Doctor>();
            CreateMap<Doctor, DoctorModel>();
            CreateMap<CreateDoctorModel, Doctor>();

            // Service category mapping
            CreateMap<UpdateServiceCategoryModel, ServiceCategory>();
            CreateMap<ServiceCategory, ServiceCategoryModel>();
            CreateMap<CreateServiceCategoryModel, ServiceCategory>();

            // Service mapping
            CreateMap<UpdateServiceModel, Service>();
            CreateMap<Service, ServiceModel>();
            CreateMap<CreateServiceModel, Service>();

            // Specialization mapping
            CreateMap<UpdateSpecializationModel, Specialization>();
            CreateMap<Specialization, SpecializationModel>();
            CreateMap<CreateSpecializationModel, Specialization>();
        }
    }
}
