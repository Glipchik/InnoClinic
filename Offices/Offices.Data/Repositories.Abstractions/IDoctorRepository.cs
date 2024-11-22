using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Offices.Data.Entities;

namespace Offices.Data.Repositories.Abstractions
{
    public interface IDoctorRepository: IGenericRepository<Doctor>
    {
        Task<IEnumerable<Doctor>> GetDoctorsFromOffice(string officeId, CancellationToken cancellationToken);
    }
}
