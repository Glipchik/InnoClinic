using Appointments.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointments.Application.Services.Abstractions
{
    public interface IPatientService
    {
        Task<PatientModel> GetByAccountId(Guid accountId, CancellationToken cancellationToken);
    }
}
