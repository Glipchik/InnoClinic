using Services.Application.Models;
using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services.Abstractions
{
    public interface IServiceService : IGeneralService<CreateServiceModel, ServiceModel, UpdateServiceModel>
    {
    }
}
