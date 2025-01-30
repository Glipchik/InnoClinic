using Profiles.API.DTOs;
using Profiles.Application.Models;
using System;

namespace Profiles.API.Mapper;

public partial class ApiMapping
{
    partial void AddPatientMapping()
    {
        CreateMap<UpdatePatientDto, UpdatePatientModel>();
        CreateMap<PatientModel, PatientDto>();
        CreateMap<CreatePatientDto, CreatePatientModel>();
        CreateMap<PatientModel, UpdatePatientModel>();
        CreateMap<UpdatePatientDto, PatientModel>();
        CreateMap<UpdatePatientByReceptionistDto, PatientModel>();
        CreateMap<CreatePatientFromAuthServerDto, CreatePatientModel>();
    }
}