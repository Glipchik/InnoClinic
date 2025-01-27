using Profiles.API.DTOs;
using Profiles.Application.Models;
using System;

namespace Profiles.API.Mapper
{
    public partial class ApiMapping
    {
        partial void AddReceptionistMapping()
        {
            CreateMap<UpdateReceptionistDto, UpdateReceptionistModel>();
            CreateMap<ReceptionistModel, ReceptionistDto>();
            CreateMap<CreateReceptionistDto, CreateReceptionistModel>();
            CreateMap<ReceptionistModel, UpdateReceptionistModel>();
            CreateMap<UpdateReceptionistDto, ReceptionistModel>();
        }
    }
}
