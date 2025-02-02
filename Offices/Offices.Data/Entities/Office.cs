using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Offices.Data.Entities
{
    public class Office: BaseEntity
    {
        [BsonRequired]
        public required string Address { get; set; }

        [BsonRequired]
        public required string RegistryPhoneNumber { get; set; }

        [BsonRequired]
        public required bool IsActive { get; set; }
    }
}
