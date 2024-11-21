using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Offices.Application.Models;

namespace Offices.Application.Services.Abstractions
{
    public interface IDoctorService
    {
        Task Create(CreateDoctorModel createDoctorModel);
        Task<DoctorModel> Get(string id);
        Task<IEnumerable<DoctorModel>> GetAll();
        Task Update(UpdateDoctorModel updateDoctorModel);
        Task Delete(string id);
    }
}