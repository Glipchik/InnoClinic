using Services.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services.Abstractions
{
    public interface ISpecializationService : IGeneralService<CreateSpecializationModel, SpecializationModel, UpdateSpecializationModel>
    {
    }
}
