using AutoMapper;

namespace Services.API.Mapper
{
    public partial class ApiMapping : Profile
    {
        partial void AddDoctorMapping();
        partial void AddServiceMapping();
        partial void AddServiceCategoryMapping();
        partial void AddSpecializationMapping();

        public ApiMapping()
        {
            AddDoctorMapping();
            AddServiceCategoryMapping();
            AddSpecializationMapping();
            AddServiceMapping();
        }
    }
}
