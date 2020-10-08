using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Nearix.HR.Core.Interfaces;
using Nearix.HR.Core.Model;
using Nearix.HR.Data;
using Nearix.HR.Service;
using Nearix.HR.Web.Models;
using Newtonsoft.Json.Serialization;
namespace Nearix.HR.Web
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
            services.AddControllersWithViews();
            services.AddControllers(options =>
            {
                options.OutputFormatters.RemoveType<StringOutputFormatter>();
                options.OutputFormatters.RemoveType<HttpNoContentOutputFormatter>();
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = 100000;
                options.ValueLengthLimit = 1073741824;
                options.MultipartBodyLengthLimit = 104857600; //100MB
            });

            
           var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Employee, NhEmployee>();
                cfg.CreateMap<EmployeeSearchModel, EmployeeSearch>();
            });
            var mapper = mapperConfiguration.CreateMapper();
            services.AddSingleton(mapper);
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

        public void ConfigureContainer(ContainerBuilder container)
        {
            container.RegisterType<EmployeeDao>().As<IEmployeeDao>().WithParameter("connectionString", Configuration["AppSettings:ConnectionString"]);
            container.RegisterType<LoggingService>().As<ILoggingService>().SingleInstance();
            container.RegisterType<FileTaskService>().As<IFileTaskService>().WithParameter("uploadDirectory", Configuration["AppSettings:UploadDirectoy"]);
        }
    }
}
