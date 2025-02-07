using Appointments.Domain.Entities;
using Appointments.Domain.Repositories.Abstractions;
using Appointments.Infrastructure.Contexts;

namespace Appointments.Infrastructure.Repositories
{
    public class ServiceCategoryRepository : GenericRepository<ServiceCategory>, IServiceCategoryRepository
    {
        public ServiceCategoryRepository(AppDbContext context) : base(context)
        {
        }
    }
}
