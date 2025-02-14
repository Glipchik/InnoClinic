using AutoMapper;
using System;

namespace Profiles.API.Mapper;

public partial class ApiMapping : Profile
{
    partial void AddDoctorMapping();
    partial void AddPatientMapping();
    partial void AddReceptionistMapping();
    partial void AddAccountMapping();
    partial void AddOfficeMapping();
    partial void AddSpecializationMapping();

    public ApiMapping()
    {
        AddDoctorMapping();
        AddPatientMapping();
        AddReceptionistMapping();
        AddAccountMapping();
        AddOfficeMapping();
        AddSpecializationMapping();
    }
}