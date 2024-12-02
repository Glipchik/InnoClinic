using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Domain.Entities;
using Services.Domain.Repositories.Abstractions;
using Services.Infrastructure.Contexts;

namespace Services.Infrastructure.Repositories
{
    public class ServiceCategoryRepository : GenericRepository<ServiceCategory>, IServiceCategoryRepository
    {
        public ServiceCategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
