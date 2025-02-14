using AutoMapper;
using Profiles.API.DTOs;
using Profiles.Application.Models;
using System;

namespace Profiles.API.Mapper;

public partial class ApiMapping : Profile
{
    partial void AddOfficeMapping()
    {
        CreateMap<OfficeModel, OfficeDto>();
    }
}
