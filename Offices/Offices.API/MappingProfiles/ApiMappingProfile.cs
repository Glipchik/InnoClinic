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
            CreateMap<UpdateOfficeModel, Office>();
            CreateMap<Office, OfficeDto>();
            CreateMap<OfficeModel, OfficeDto>();
            CreateMap<CreateOfficeDto, CreateOfficeModel>();
            CreateMap<UpdateOfficeDto, UpdateOfficeModel>();

            // Doctors mapping
            CreateMap<UpdateDoctorModel, Doctor>();
            CreateMap<Doctor, DoctorModel>();
            CreateMap<CreateDoctorModel, Doctor>();

            // Receptionists mapping
            CreateMap<UpdateReceptionistModel, Receptionist>();
            CreateMap<Receptionist, ReceptionistModel>();
            CreateMap<CreateReceptionistModel, Receptionist>();
        }
    }
}
