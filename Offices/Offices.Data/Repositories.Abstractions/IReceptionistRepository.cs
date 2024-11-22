using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Offices.Data.Entities;

namespace Offices.Data.Repositories.Abstractions
{
    public interface IReceptionistRepository: IGenericRepository<Receptionist>
    {
        Task<IEnumerable<Receptionist>> GetReceptionistsFromOffice(string officeId, CancellationToken cancellationToken);
    }
}
