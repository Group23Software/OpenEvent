using System;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using OpenEvent.Web.Contexts;
using OpenEvent.Web.Hubs;
using OpenEvent.Web.Services;
using OpenEvent.Web.UserOwnsEvent;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Prometheus;

namespace OpenEvent.Web
{
    /// <summary>
    /// Class to configuring services and the app
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="env"></param>
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        private IConfiguration Configuration { get; }
        private IWebHostEnvironment Environment { get; }

        /// <summary>
        /// All dependency injection configuration happens here
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Get app settings from appsettings.json
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            services.AddResponseCompression(options => { options.Providers.Add<GzipCompressionProvider>(); });
            services.AddLogging(loggingBuilder => { loggingBuilder.AddSeq(); });

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

            // Add Db context using connection string from app settings.
            services.AddDbContextPool<ApplicationContext>(options =>
                options.UseMySql(appSettings.ConnectionString,
                    new MySqlServerVersion(new Version(8, 0, 23)),
                    mySqlOptions => mySqlOptions.CharSetBehavior(CharSetBehavior.NeverAppend)));

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
            services.AddAutoMapper(typeof(Startup));

            // Add signalR with pascal naming scheme for web-sockets 
            services.AddSignalR(options =>
            {
                
            }).AddJsonProtocol(options =>
            {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            });

            // Register singleton services (singletons are instantiated once and only) 
            services.AddSingleton<IWorkQueue, WorkQueue>();
            services.AddSingleton<IAnalyticsService, AnalyticsService>();
            services.AddSingleton<IRecommendationService, RecommendationService>();
            services.AddSingleton<IPopularityService, PopularityService>();
            services.AddSingleton<IEmailService, EmailService>();
            
            // Register hosted services for background work
            services.AddHostedService<BackGroundWorkService>();
            services.AddHostedService<PopularityWatchDog>();

            // Register scoped services (scoped services are instantiated on every request)
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IBankingService, BankingService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IPromoService, PromoService>();
            
            services.AddScoped<UserOwnsEventFilter>();

            // Inject http client into the event service
            services.AddHttpClient<IEventService, EventService>();

            services.AddLogging();
            
            services.AddHttpContextAccessor();

            // Registering all controllers
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                }
            );

            // services.AddStackExchangeRedisCache(options =>
            // {
            //     options.Configuration = "localhost:6379";
            // });

            services.AddSwaggerGen();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "ClientApp/dist"; });
        }
        
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseHttpsRedirection();
            }
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseResponseCompression();
            app.UseRouting();
            app.UseHttpMetrics();

            if (env.IsDevelopment())
            {
                app.UseCors("AllowOrigin");
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();

            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "OpenEvent"); });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");

                endpoints.MapHub<UserHub>("/userHub");
                endpoints.MapHub<PopularityHub>("/popularityHub");

                endpoints.MapMetrics();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    // spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}