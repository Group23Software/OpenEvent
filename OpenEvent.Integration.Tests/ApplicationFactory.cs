using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using OpenEvent.Web;
using OpenEvent.Web.Contexts;

namespace OpenEvent.Integration.Tests
{
    // public class TestStartup : Startup
    // {
    //     public TestStartup(IConfiguration configuration) : base(configuration)
    //     {
    //     }
    //
    //     public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    //     {
    //         // if (env.IsDevelopment())
    //         // {
    //         app.UseDeveloperExceptionPage();
    //         // }
    //         // else
    //         // {
    //         //     app.UseExceptionHandler("/Error");
    //         //     app.UseHsts();
    //         // }
    //
    //         app.UseStaticFiles();
    //
    //         app.UseRouting();
    //
    //         app.UseCors("AllowOrigin");
    //
    //         app.UseAuthentication();
    //
    //         app.UseSwagger();
    //
    //         app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenEvent"); });
    //
    //         app.UseEndpoints(endpoints =>
    //         {
    //             endpoints.MapControllerRoute(
    //                 name: "default",
    //                 pattern: "{controller}/{action=Index}/{id?}");
    //         });
    //     }
    // }

    public class TestStartup
    {
        private IConfiguration Configuration { get; }

        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // base.ConfigureServices(services);
            
            // Get app settings from appsettings.json
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            // Add cors so angular dev server can make requests.
            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod()
                            .AllowCredentials();
                    });
            });

            services.AddDbContext<ApplicationContext>(options => { options.UseInMemoryDatabase("OpenEventTesting"); });

            // Register JWT using secret from app settings.
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            // Add automapping configuration.
            services.AddAutoMapper(typeof(TestStartup));

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                }
            );
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // base.Configure(app, env);
            
            app.UseDeveloperExceptionPage();

            // app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowOrigin");

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
        }
    }

    public class ApplicationFactory<TStartup>
        : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(config =>
            {
                var integrationConfig = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .Build();

                config.AddConfiguration(integrationConfig);
            });
        }

        protected override IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder(null)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<TestStartup>(); });
        }
    }
}