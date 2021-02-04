using Infrastructure.Contracts.Request;
using Application.Interfaces;
using Application.Mappers;
using Application.Validators;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WebApi.Business;

namespace WebApi
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

            services.AddControllers()
                    .AddFluentValidation();

            services.AddAutoMapper(typeof(ClusterMapperProfile).Assembly);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });

            services.AddSharedInfrastructure();
            services.AddPersistenceInfrastructure(Configuration);

            AddBusinessLayer(services);
        }

        private void AddBusinessLayer(IServiceCollection services)
        {
            services.AddScoped<IClusterBusiness, ClusterBusiness>();

            services.AddTransient<IValidator<ClusterUpdateRequest>, ClusterUpdateRequestValidator>();
            services.AddTransient<IValidator<ClusterCreateRequest>, ClusterCreateRequestValidator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ServiceDbContext clusterDbContext)
        {
            if (clusterDbContext.Database.EnsureCreated())
            {

            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
