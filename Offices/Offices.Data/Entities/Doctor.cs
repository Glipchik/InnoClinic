using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Offices.Data.Enums;

namespace Offices.Data.Entities
{
    public class Doctor : BaseEntity
    {
        [BsonRequired]
        public required string FirstName { get; set; }

        [BsonRequired]
        public required string LastName { get; set; }

        public string? MiddleName { get; set; }

        [BsonRequired]
        [BsonRepresentation(BsonType.String)]

        public required Guid OfficeId { get; set; }

        [BsonRequired]
        public required DoctorStatus Status { get; set; }
    }
}
