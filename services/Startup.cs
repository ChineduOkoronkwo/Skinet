using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using AutoMapper;
using services.Helpers;
using services.Middleware;
using services.Extensions;
using StackExchange.Redis;
using Infrastructure.Identity;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace services
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;

        }

        public void ConfigureDevelopmentServices(IServiceCollection services) 
        {
            // Inject the shop database
            services.AddDbContext<StoreContext>(
                option => option.UseSqlite(_config.GetConnectionString("DefaultConnection"))); 

            // Inject identity database
            services.AddDbContext<AppIdentityDbContext>(
                option => option.UseSqlite(_config.GetConnectionString("IdentityConnection"))); 

            ConfigureServices(services);
        }

        public void ConfigureProductionServices(IServiceCollection services) 
        {
            // Inject the shop database
            services.AddDbContext<StoreContext>(
                option => option.UseMySql(_config.GetConnectionString("DefaultConnection"))); 

            // Inject identity database
            services.AddDbContext<AppIdentityDbContext>(
                option => option.UseMySql(_config.GetConnectionString("IdentityConnection"))); 

            ConfigureServices(services);
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Inject shopping basket database
            services.AddSingleton<IConnectionMultiplexer>(c => {
                var configuration = ConfigurationOptions.Parse(
                    _config.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });        
            
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddApplicationServices();
            services.AddIdentityServices(_config);
            services.AddSwaggerDocumentation();

            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", policy => {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "Content")
                ), RequestPath="/Content"
            });

            app.UseCors("CorsPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UserSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToController("Index", "Fallback");
            });
        }
    }
}
