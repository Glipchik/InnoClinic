using AutoMapper;

namespace Offices.Data.Mapper
{
    public partial class DataMapping : Profile
    {
        partial void AddOfficeEventMapping();

        public DataMapping()
        {
            AddOfficeEventMapping();
        }
    }
}
