using Application.Interfaces;
using Application.Mappers;
using AutoMapper;
using Datacenter.Service.Business;
using FluentValidation.AspNetCore;
using Infrastructure.Persistence;
using Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Datacenter.Service
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                               .AddFluentValidation();

            services.AddAutoMapper(typeof(DatacenterMapperProfile).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Datacenter.Service", Version = "v1" });
            });

            services.AddSharedInfrastructure();
            services.AddPersistenceInfrastructure(Configuration);
            services.AddCors(cors =>
            {
                cors.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
            AddBusinessLayer(services);
        }

        private void AddBusinessLayer(IServiceCollection services)
        {
            services.AddScoped<IDatacenterBusiness, DatacenterBusiness>();
            services.AddScoped<ISshKeyBusiness, SshKeyBusiness>();
            services.AddScoped<ITemplateBusiness, TemplateBusiness>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Datacenter.Service v1"));
            }

            app.UseHttpsRedirection();

            app.UseCors();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
