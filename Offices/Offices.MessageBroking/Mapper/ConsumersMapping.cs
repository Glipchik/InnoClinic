using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Consumers.Mapper
{
    public partial class ConsumersMapping
    {
        partial void AddDoctorMapping();
        partial void AddReceptionistMapping();

        public ConsumersMapping()
        {
            AddReceptionistMapping();
            AddDoctorMapping();
        }
    }
}
