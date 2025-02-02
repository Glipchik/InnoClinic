using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Profiles.Application.Models
{
    public class CreateReceptionistModel
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public Guid OfficeId { get; set; }
    }

    public class ReceptionistModel
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public Guid AccountId { get; set; }
        public Guid OfficeId { get; set; }
    }

    public class UpdateReceptionistModel
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string MiddleName { get; set; }
        public Guid OfficeId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
