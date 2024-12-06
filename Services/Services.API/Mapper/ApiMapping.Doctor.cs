using Services.API.DTOs;
using Services.Application.Models;
using Services.Domain.Entities;

namespace Services.API.Mapper
{
    public partial class ApiMapping
    {
        partial void AddDoctorMapping()
        {
            CreateMap<UpdateDoctorDto, UpdateDoctorModel>();
            CreateMap<DoctorModel, DoctorDto>();
            CreateMap<CreateDoctorDto, CreateDoctorModel>();
        }
    }
}
