using Appointments.Application.Mapper;
using Appointments.Application.Services;
using Appointments.Application.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Appointments.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplicationDependencyInjection(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(ApplicationMapping));

            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IDoctorService, DoctorService>();

            return services;
        }
    }
}
