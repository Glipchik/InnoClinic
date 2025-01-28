using AutoMapper;
using Profiles.Application.Models;
using Profiles.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Mapper
{
    public partial class ApplicationMapping : Profile
    {
        partial void AddDoctorMapping();
        partial void AddPatientMapping();
        partial void AddReceptionistMapping();
        partial void AddAccountMapping();

        public ApplicationMapping()
        {
            AddDoctorMapping();
            AddPatientMapping();
            AddReceptionistMapping();
            AddAccountMapping();
        }
    }
}
