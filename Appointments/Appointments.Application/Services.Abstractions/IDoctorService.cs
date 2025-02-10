using Appointments.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Application.Services.Abstractions
{
    public interface IDoctorService
    {
        Task<DoctorModel> GetByAccountId(Guid accountId, CancellationToken cancellationToken);
    }
}
