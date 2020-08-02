using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using museum.Application.automapper;
using museum.Application.helpers;
using museum.Application.main;
using museum.Application.main.chronicles;
using museum.Application.main.classes;
using museum.Application.main.events;
using museum.Application.main.graduates;
using museum.Application.main.mailing;
using museum.Application.main.photos;
using museum.Application.main.students;
using museum.Application.main.teachers;
using museum.Application.main.users;
using museum.EF.repositories;
using museumApi.EF.entities;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using System.Text;

namespace museumApi
{
    public class Startup
    {
        readonly IConfigurationRoot _config;
        private readonly IHostingEnvironment _env;
        public IConfiguration _configuration { get; }

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                //konfiguracinis failas
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _env = env;
            _config = builder.Build();
            _configuration = configuration;

            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(x => x.AddProfile(new MappingsProfile()));
            //uzregistruojam konteksto klase
            services.AddDbContext<MuseumContext>(options =>
            options.UseSqlServer(_configuration.GetConnectionString("MuseumDatabase")));

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(_configuration.GetConnectionString("MuseumDatabase"));

            });
            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
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


            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IUsersService, UsersService>();

            services.AddScoped<IClassesRepository, ClassesRepository>();
            services.AddScoped<IClassesService, ClassesService>();

            services.AddScoped<ITeachersRepository, TeachersRepository>();
            services.AddScoped<ITeachersService, TeachersService>();

            services.AddScoped<IStudentsRepository, StudentsRepository>();
            services.AddScoped<IStudentsService, StudentsService>();

            services.AddScoped<IChroniclesRepository, ChroniclesRepository>();
            services.AddScoped<IChroniclesService, ChroniclesService>();

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventService, EventService>();

            services.AddScoped<IGraduatesRepository, GraduatesRepository>();
            services.AddScoped<IGraduatesService, GraduatesService>();

            services.AddScoped<IMailingService, MailingService>();

            services.AddScoped<IPhotosRepository, PhotosRepository>();

            services.AddCors(options => options.AddPolicy("AllowAll",
            builder =>
            {
                builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            }));
            services.AddMvc();
            //.AddJsonOptions(
           // options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // veeikia su sitaais
            app.UseCors("AllowAll");
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
                RequestPath = new PathString("/Resources")
            });
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI V1");
            });

            app.UseHangfireServer(new BackgroundJobServerOptions()
            {
                WorkerCount = 2,
            });

            RecurringJob.AddOrUpdate<IMailingService>(
                e => e.ManageQueue(),
                Cron.MinuteInterval(2));

            app.UseHangfireDashboard();
        }
    }
}
