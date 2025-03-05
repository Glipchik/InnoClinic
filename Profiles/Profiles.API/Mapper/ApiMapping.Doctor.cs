using Profiles.API.DTOs;
using Profiles.Application.Models;
using System;

namespace Profiles.API.Mapper;

public partial class ApiMapping
{
    partial void AddDoctorMapping()
    {
        CreateMap<UpdateDoctorDto, UpdateDoctorModel>();
        CreateMap<DoctorModel, DoctorDto>();
        CreateMap<CreateDoctorDto, CreateDoctorModel>();
        CreateMap<DoctorModel, UpdateDoctorModel>();
        CreateMap<UpdateDoctorDto, DoctorModel>();
        CreateMap<UpdateDoctorByReceptionistDto, DoctorModel>();
        CreateMap<DoctorQueryParametresDto, DoctorQueryParametresModel>();
    }
}
