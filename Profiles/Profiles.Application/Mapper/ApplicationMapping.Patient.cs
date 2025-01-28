using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Profiles.Application.Models;
using Profiles.Domain.Entities;

namespace Profiles.Application.Mapper
{
    partial class ApplicationMapping : Profile
    {
        partial void AddPatientMapping()
        {
            CreateMap<UpdatePatientModel, Patient>();
            CreateMap<Patient, PatientModel>();
            CreateMap<CreatePatientModel, Patient>();
        }
    }
}
