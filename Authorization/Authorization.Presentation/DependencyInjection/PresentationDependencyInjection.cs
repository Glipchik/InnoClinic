using Authorization.Application.DependencyInjection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.EntityFramework.Mappers;

namespace Authorization.Presentation.DependencyInjection
{
    public static class PresentationDependencyInjection
    {
        public static IServiceCollection AddPresentationDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddProblemDetails();

            services.AddApplicationDependencyInjection(configuration);

            services.AddRazorPages();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            var isBuilder = services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.EmitStaticAudienceClaim = true;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString, dbOpts => dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName));
                })
                // this is something you will want in production to reduce load on and requests to the DB
                //.AddConfigurationStoreCache()
                //
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString, dbOpts => dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName));
                });

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                dbContext.Database.Migrate();
                AddClient(dbContext, configuration);
                AddScopes(dbContext);
                var operationalDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                operationalDbContext.Database.Migrate();
            }
            
            return services;
        }

        private static void AddClient(ConfigurationDbContext context, IConfiguration configuration)
        {
            var servicesApiClient = new Client
            {
                ClientId = configuration.GetSection("AuthorizationClients").GetSection("ServicesApi")["ClientId"]!,
                ClientName = configuration.GetSection("AuthorizationClients").GetSection("ServicesApi")["ClientName"],
                ClientSecrets = { new Secret(configuration.GetSection("AuthorizationClients").GetSection("ServicesApi")["ClientSecret"].Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { $"{configuration.GetSection("AuthorizationClients").GetSection("ServicesApi")["ClientBaseUrl"]}signin-oidc" },
                PostLogoutRedirectUris = { $"{configuration.GetSection("AuthorizationClients").GetSection("ServicesApi")["ClientBaseUrl"]}signout-callback-oidc" },
                AllowedScopes = { "api.read", "api.write", "profile", "openid", "email" }
            };

            if (!context.Clients.Any(c => c.ClientId == servicesApiClient.ClientId))
            {
                context.Clients.Add(servicesApiClient.ToEntity());
                context.SaveChanges();
            }
        }

        private static void AddScopes(ConfigurationDbContext context)
        {
            var scopes = new List<ApiScope>
            {
                new ApiScope("openid", "OpenID Connect"),
                new ApiScope("profile", "User Profile"),
                new ApiScope("email", "User Email"),
                new ApiScope("api.read", "Read access to API"),
                new ApiScope("api.write", "Write access to API")
            };

            foreach (var scope in scopes)
            {
                if (!context.ApiScopes.Any(s => s.Name == scope.Name))
                {
                    context.ApiScopes.Add(scope.ToEntity());
                    Console.WriteLine($"Added API scope: {scope.Name}");
                }
            }

            context.SaveChanges();
        }

    }
}
