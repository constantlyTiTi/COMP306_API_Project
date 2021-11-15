using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using apiProject.DBContexts;
using apiProject.Interfaces;
using apiProject.Models;
using apiProject.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
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
            services.AddAutoMapper(typeof(Startup));
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
            //Add autoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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
