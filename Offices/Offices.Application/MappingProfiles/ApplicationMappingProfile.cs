using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Offices.Application.Models;
using Offices.Data.Entities;

namespace Offices.Application.MappingProfiles
{
    public class ApplicationMappingProfile: Profile
    {
        public ApplicationMappingProfile()
        {
            // Offices mapping
            CreateMap<UpdateOfficeModel, Office>();
            CreateMap<Office, OfficeModel>();
            CreateMap<CreateOfficeModel, Office>();

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
