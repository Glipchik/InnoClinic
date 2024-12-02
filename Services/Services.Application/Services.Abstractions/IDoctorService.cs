using Services.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services.Abstractions
{
    public interface IDoctorService : IGenericService<CreateDoctorModel, DoctorModel, UpdateDoctorModel>
    {
    }
}
