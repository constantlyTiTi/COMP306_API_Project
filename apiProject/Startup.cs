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
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            //Register autoMapper
            //Add autoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //get data from parameter store
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
            services.AddScoped<IRateRepo, RateRepo>();
            services.AddScoped<IOrderDetailsRepo, OrderDetailsRepo>();
            services.AddScoped<IOrderItemRepo, OrderItemRepo>();
            services.AddScoped<IItemFileRepo, ItemFileRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            // Add Role services to Identity
            services.AddDefaultIdentity<User>(
                options => {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireLowercase = false;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;

                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<MSSQLDbContext>();
            //Cookie Authentication middleware redirects the User, if he is not authenticated
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                options.LoginPath = "/Identity/Login";
                options.LogoutPath = "/";
                options.AccessDeniedPath = "/Identity/AccessDenied";
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
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
