using Events.Office;
using Offices.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Data.Mapper
{
    public partial class DataMapping
    {
        partial void AddOfficeEventMapping()
        {
            CreateMap<Office, OfficeCreated>();
            CreateMap<Office, OfficeUpdated>();
        }
    }
}
