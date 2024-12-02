using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Services.Domain.Entities;

namespace Services.Infrastructure.Contexts
{
    public class AppDbContext : DbContext
    {
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceCategory> ServiceCategories { get; set; }
        public DbSet<Specialization> Specializations { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }
    }
}
