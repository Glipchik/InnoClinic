using AutoMapper;
using Profiles.Application.Models;
using Profiles.Domain.Entities;
using System;

namespace Profiles.Application.Mapper;

partial class ApplicationMapping : Profile
{
    partial void AddOfficeMapping()
    {
        CreateMap<Office, OfficeModel>();
    }
}