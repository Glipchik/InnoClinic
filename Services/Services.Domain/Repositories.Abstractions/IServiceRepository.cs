﻿using Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Domain.Repositories.Abstractions
{
    public interface IServiceRepository : IGenericRepository<Service>
    {
        Task<IEnumerable<Service>> GetAllAsync(Guid? seviceCategoryId, Guid? specializationId, bool? isActive, CancellationToken cancellationToken);
    }
}
