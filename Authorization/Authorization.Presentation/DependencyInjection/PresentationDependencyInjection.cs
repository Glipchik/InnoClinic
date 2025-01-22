using Authorization.Application.DependencyInjection;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.EntityFramework.Mappers;
using FluentValidation;
using IdentityModel;
using Authorization.Presentation.Validators;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using Authorization.Presentation.Infrastructure;

namespace Authorization.Presentation.DependencyInjection
{
    public static class PresentationDependencyInjection
    {
        public static IServiceCollection AddPresentationDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddProblemDetails();

            services.AddExceptionHandler<GlobalExceptionHandler>();

            services.AddApplicationDependencyInjection(configuration);

            services.AddControllersWithViews();

            services.AddRazorPages();

            services.AddFluentValidationAutoValidation()
                    .AddValidatorsFromAssemblyContaining<Program>();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services
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
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b =>
                        b.UseSqlServer(connectionString, dbOpts => dbOpts.MigrationsAssembly(typeof(Program).Assembly.FullName));
                });

             services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = configuration["AuthorizationServerUrl"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false
                    };
                })
                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = configuration["AuthorizationServerUrl"];
                    options.ClientId = "client_id";
                    options.ClientSecret = "client_secret";
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("email");
                    options.Scope.Add("roles");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireCreateAccountScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "create_account");
                    policy.AuthenticationSchemes.Add("Bearer");
                });
            });

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                dbContext.Database.Migrate();
                AddClient(dbContext, configuration);
                AddScopes(dbContext);
                AddIdentityResources(dbContext);
                AddApiResources(dbContext);
                var operationalDbContext = scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>();
                operationalDbContext.Database.Migrate();
            }

            return services;
        }

        private static void AddClient(ConfigurationDbContext context, IConfiguration configuration)
        {
            var clients = new List<Client>()
            {
                GetClient(configuration, "ServicesApi"),
                GetClient(configuration, "OfficesApi"),
                GetClient(configuration, "ProfilesApi"),
                new Client
                {
                    ClientId = configuration.GetSection("AuthorizationClients").GetSection("ProfilesAuthM2M")["ClientId"]!,
                    ClientName = configuration.GetSection("AuthorizationClients").GetSection("ProfilesAuthM2M")["ClientName"],
                    ClientSecrets = { new Secret(configuration.GetSection("AuthorizationClientSecrets").GetSection("ProfilesAuthM2M")["ClientSecret"].Sha256()) },
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    AllowedScopes = { "create_account" }
                },
                new Client
                {
                    ClientId = configuration["AuthorizationClients:AuthProfilesM2M:ClientId"],
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = { new Secret(configuration["AuthorizationClientSecrets:AuthProfilesM2M:ClientSecret"].Sha256()) },
                    AllowedScopes = { "create_patient_profile" }
                }
            };

            foreach (var client in clients)
            {
                if (!context.Clients.Any(c => c.ClientId == client.ClientId))
                {
                    context.Clients.Add(client.ToEntity());
                }
            }
        }

        private static Client GetClient(IConfiguration configuration, string clientName)
        {
            return new Client
            {
                ClientId = configuration.GetSection("AuthorizationClients").GetSection(clientName)["ClientId"]!,
                ClientName = configuration.GetSection("AuthorizationClients").GetSection(clientName)["ClientName"],
                ClientSecrets = { new Secret(configuration.GetSection("AuthorizationClientSecrets").GetSection(clientName)["ClientSecret"].Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = { $"{configuration.GetSection("AuthorizationClients").GetSection(clientName)["ClientBaseUrl"]}signin-oidc" },
                PostLogoutRedirectUris = { $"{configuration.GetSection("AuthorizationClients").GetSection(clientName)["ClientBaseUrl"]}signout-callback-oidc" },
                AllowedScopes =
                {
                    "openid",
                    "profile",
                    "email",
                    "roles",
                    "api_profile",
                    "api_email",
                    "api_roles"
                },
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = true
            };
        }


        private static void AddIdentityResources(ConfigurationDbContext context)
        {
            var identityResources = new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("email", new[] { JwtClaimTypes.Email }),
                new IdentityResource("roles", new[] { JwtClaimTypes.Role })
            };

            foreach (var resource in identityResources)
            {
                if (!context.IdentityResources.Any(ir => ir.Name == resource.Name))
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
            }

            context.SaveChanges();
        }

        private static void AddApiResources(ConfigurationDbContext context)
        {
            var apiResources = new List<ApiResource>
            {
                new ApiResource("create_patient_profile", "Create patient")
            };

            foreach (var resource in apiResources)
            {
                if (!context.ApiResources.Any(ir => ir.Name == resource.Name))
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
            }

            context.SaveChanges();
        }

        private static void AddScopes(ConfigurationDbContext context)
        {
            var scopes = new List<ApiScope>
            {
                new ApiScope("api_profile", "User Profile", new[] { JwtClaimTypes.Email, JwtClaimTypes.Role }),
                new ApiScope("api_email", "Access to email", new[] { JwtClaimTypes.Email }),
                new ApiScope("api_roles", "Access to roles", new[] { JwtClaimTypes.Role }),
                new ApiScope("create_account", "Access to creating accounts"),
                new ApiScope("create_patient_profile", "Create patient")
            };

            foreach (var scope in scopes)
            {
                if (!context.ApiScopes.Any(s => s.Name == scope.Name))
                {
                    context.ApiScopes.Add(scope.ToEntity());
                }
            }

            context.SaveChanges();
        }



    }
}