using AutoMapper;
using Offices.API.DTOs;
using Offices.Application.Models;
using Offices.Data.Entities;

namespace Offices.API.MappingProfiles
{
    public class ApiMappingProfile: Profile
    {
        public ApiMappingProfile() 
        {
            // Offices mapping
            CreateMap<OfficeModel, OfficeDto>();
            CreateMap<CreateOfficeDto, CreateOfficeModel>();
            CreateMap<UpdateOfficeDto, UpdateOfficeModel>();

            // Doctors mapping
            CreateMap<DoctorModel, DoctorDto>();
            CreateMap<CreateDoctorDto, CreateDoctorModel>();
            CreateMap<UpdateDoctorDto, UpdateDoctorModel>();

            // Receptionists mapping
            CreateMap<ReceptionistModel, ReceptionistDto>();
            CreateMap<CreateReceptionistDto, CreateReceptionistModel>();
            CreateMap<UpdateReceptionistDto, UpdateReceptionistModel>();
        }
    }
}
