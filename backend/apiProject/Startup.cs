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
using System.Text;
using Microsoft.IdentityModel.Tokens;
using apiProject.TokenAuth;
using Microsoft.AspNetCore.HttpOverrides;

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

            //registers authentication services and handlers for cookie and JWT bearer authentication schemes
            /*services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => Configuration.Bind("JwtSettings", options))
                    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => Configuration.Bind("CookieSettings", options));*/
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(jwt =>
                {
                    var key = Encoding.ASCII.GetBytes(config.JwtSecretToken);
                    jwt.SaveToken = true;
                    //Define configuration of JWT 
                    jwt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Validate the third part of jwt token using the secret we have generated
                        ValidateIssuerSigningKey = true,
                        //define signing key
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        //will change after deployment
                        RequireExpirationTime = false
                    };
                });
            //register the Identity related services. To do that we use the AddIdentity extension method
            services.AddDefaultIdentity<IdentityUser>(
                options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                }
                ).AddEntityFrameworkStores<MSSQLDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenManager, TokenManager>();

            services.AddMvc(config =>
            {
                config.FormatterMappings.SetMediaTypeMappingForFormat("xml", "application/xml");
                config.FormatterMappings.SetMediaTypeMappingForFormat("js", "application/json");
            });

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

            //config proxy
            services.AddControllersWithViews();
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //config proxy
            app.UseForwardedHeaders();
            /*app.MapWhen(ctx=>ctx.Request.Path.StartsWithSegments("/home"), builder => builder.RunProxy())*/
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "project_api v1"));
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

/*        private static GetParameterResponse GetConfiguration()
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
        }*/
    }
}
