using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using apiProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Features;

namespace apiProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "project_api", Version = "v1" });
            });
            //Register autoMapper
            //Add autoMapper
            services.AddAutoMapper(typeof(Startup));

            //get data from parameter store
/*            var result = GetConfiguration().Result;*/
            services.Configure<ProjectPSConfig>(Configuration.GetSection(ProjectPSConfig.SectionName));
            ProjectPSConfig config = Configuration.GetSection(ProjectPSConfig.SectionName).Get<ProjectPSConfig>();
            //set aws options
            var awsOptions = new AWSOptions
            {
                Region = RegionEndpoint.USEast1

            };
            awsOptions.Credentials = new BasicAWSCredentials(config.AccessKeyID, config.SecretAccessKey);
            services.AddDefaultAWSOptions(awsOptions);
            //allow independant injection
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonDynamoDB>();
            //get connection string
            string connectionString = config.ConnectionString
                + $"User ID = {config.RDSMasterAccount}; Password = {config.RDSPassword}";
            services.AddDbContext<MSSQLDbContext>(opt => opt.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMvc(config =>
            {
                config.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
                config.FormatterMappings.SetMediaTypeMappingForFormat("js", "application/json");
            });

            //registers authentication services and handlers for cookie and JWT bearer authentication schemes
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("JwtSettings", options))
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => Configuration.Bind("CookieSettings", options));

            //register the Identity related services. To do that we use the AddIdentity extension method
            services.AddIdentity<IdentityUser, IdentityRole>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                }
                ).AddEntityFrameworkStores<MSSQLDbContext>();

            // Add Role services to Identity
            services.Configure<IdentityOptions>(
                options =>
                {
                    // Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;

                    // Lockout settings.
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;

                    // User settings.
                    options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = false;

                });
            //Cookie Authentication middleware redirects the User, if he is not authenticated
             services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/home/login";
                options.LogoutPath = "/";
/*                options.AccessDeniedPath = "/Identity/AccessDenied";*/
                options.SlidingExpiration = true;
            });
            //Require authenticated users
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "project_api v1"));
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
/*            app.Run(async context =>
            {
                context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = 1048576000;
            });*/
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        private static async Task<GetParameterResponse> GetConfiguration()
        {
            // NOTE: set the region here to match the region used when you created
            // the parameter
            var region = Amazon.RegionEndpoint.USEast1;
            var request = new GetParameterRequest()
            {
                Name = @"name:/comp306/lab03/SecretAccessKey"
            };

            var client = new AmazonSimpleSystemsManagementClient(region);


            var response = client.GetParameterAsync(request).GetAwaiter().GetResult();
            return response;
        }
    }
}
