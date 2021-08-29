using System.Reflection;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Sedre.Pollution.Domain.Interfaces;
using Sedre.Pollution.Infrastructure;
using Sedre.Pollution.Infrastructure.Implementations;

namespace Sedre.Pollution.Api
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            
            services.AddHttpContextAccessor();
            
            services.AddControllers();
            
            services.AddAutoMapper(typeof(Startup));
            
            services.AddDbContext<PollutionDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Pollution")));
            
            services.AddScoped<IUnitOfWork>(provider => provider.GetService<PollutionDbContext>());
            services.AddScoped<DbContext>(provider => provider.GetService<PollutionDbContext>());
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            services.AddCors(options =>
            {
                options.AddPolicy("allowall", policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });  
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Tutor Student APIs", 
                    Version = "v1",
                    Description =  "Tutor Student APIs description"
                });
                
            });
            
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,PollutionDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            dbContext.Database.Migrate();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            
            app.UseCors("allowall");
            
            app.UseSwagger(options => { options.RouteTemplate = "api-docs/{documentName}/swagger.json"; });

            app.UseSwaggerUI(options =>
                {
                    options.RoutePrefix = "api-docs";
                    options.DocumentTitle = "Tutor Student APIs";
                    options.SwaggerEndpoint("v1/swagger.json", "Tutor Student definition");
                    options.OAuthClientId("swaggerapiui");
                    options.OAuthAppName("Swagger API UI");
                }
            );
            
            
            app.UseRouting();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}