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
                AddClient(dbContext);
                AddScopes(dbContext);
                var operationalDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                operationalDbContext.Database.Migrate();
            }
            
            return services;
        }

        private static void AddClient(ConfigurationDbContext context)
        {
            var client = new Client
            {
                ClientId = "your-client-id",
                ClientName = "Your Client Name",
                ClientSecrets = { new Secret("your-client-secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { "http://localhost:5201/signin-oidc", "http://localhost:13378/signin-oidc" },
                PostLogoutRedirectUris = { "http://localhost:5201/signout-callback-oidc", "http://localhost:13378/signout-callback-oidc" },
                AllowedScopes = { "openid", "profile", "api1", "email" }
            };

            if (!context.Clients.Any(c => c.ClientId == client.ClientId))
            {
                context.Clients.Add(client.ToEntity());
                context.SaveChanges();
            }
        }

        private static void AddScopes(ConfigurationDbContext context)
        {
            if (!context.IdentityResources.Any(r => r.Name == "openid"))
            {
                context.IdentityResources.Add(new IdentityResource
                {
                    Name = "openid",
                    DisplayName = "OpenID Connect",
                    UserClaims = { "sub" }
                }.ToEntity());
            }

            if (!context.IdentityResources.Any(r => r.Name == "profile"))
            {
                context.IdentityResources.Add(new IdentityResource
                {
                    Name = "profile",
                    DisplayName = "Profile",
                    UserClaims = { "name", "given_name", "family_name" }
                }.ToEntity());
            }

            if (!context.IdentityResources.Any(r => r.Name == "email"))
            {
                context.IdentityResources.Add(new IdentityResource
                {
                    Name = "email",
                    DisplayName = "Email",
                    UserClaims = { "email" }
                }.ToEntity());
            }

            if (!context.ApiScopes.Any(s => s.Name == "api1"))
            {
                context.ApiScopes.Add(new ApiScope
                {
                    Name = "api1",
                    DisplayName = "My API"
                }.ToEntity());
            }

            context.SaveChanges();
        }
    }
}
