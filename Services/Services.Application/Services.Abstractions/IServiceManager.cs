using Services.Application.Models;
using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Application.Services.Abstractions
{
    public interface IServiceManager : IGenericService<CreateServiceModel, ServiceModel, UpdateServiceModel>
    {
    }
}
