using AutoMapper;
using Services.Application.Models;
using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Mapper
{
    public partial class ApplicationMapping : Profile
    {
        partial void AddDoctorMapping();
        partial void AddServiceMapping();
        partial void AddServiceCategoryMapping();
        partial void AddSpecializationMapping();

        public ApplicationMapping()
        {
            AddDoctorMapping();
            AddServiceCategoryMapping();
            AddSpecializationMapping();
            AddServiceMapping();
        }
    }
}
