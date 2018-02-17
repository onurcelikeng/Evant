using Evant.Auth;
using Evant.DAL.EF;
using Evant.DAL.Interfaces.Repositories;
using Evant.DAL.Repositories;
using Evant.DAL.Repositories.Interfaces;
using Evant.Helpers;
using Evant.Interfaces;
using Evant.NotificationCenter;
using Evant.NotificationCenter.Interfaces;
using Evant.NotificationCenter.Settings;
using Evant.Storage;
using Evant.Storage.Interfaces;
using Evant.Storage.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Evant
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            // Add MVCd
            services.AddMvc();

            // Add DBContext
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DevDbConnection")));

            // Repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IFriendOperationRepository, FriendOperationRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IEventOperationRepository, EventOperationRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Scoped
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped<ILogHelper, LogHelper>();

            // OneSignal
            services.AddScoped<IOneSignal>(factory =>
            {
                return new OneSignal(new OneSignalSetting(
                    appId: Configuration["OneSignal:AppId"],
                    restApiKey: Configuration["OneSignal:RestApiKey"]));
            });

            // Azure Storage
            services.AddScoped<IAzureBlobStorage>(factory =>
            {
                return new AzureBlobStorage(new AzureBlobSeting(
                    storageAccount: Configuration["AzureStorage:StorageAccount"],
                    storageKey: Configuration["AzureStorage:StorageKey"],
                    eventContainer: Configuration["AzureStorage:Events_Container"],
                    userContainer: Configuration["AzureStorage:Users_Container"]));
            });

            // Identity
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            // Jwt Authentication
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(configure =>
                {
                    configure.RequireHttpsMetadata = false;
                    configure.SaveToken = true;
                    configure.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SecretKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Evant API",
                    Description = "ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "Onur Celik", Url = "https://github.com/onurcelikeng/Evant" }
                });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "token"
                });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            //Use Authentication
            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Evant API V1");             
            });

            app.UseMvc();
        }

    }
}
