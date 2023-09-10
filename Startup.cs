using Cart.Repository;
using CartApi.Data;
using CartApi.Domain.Entities;
using CartApi.Interfaces;
using CartApi.Middlewares;
using CartApi.Services;
using HotelApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System;

namespace CartApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        //configure logging
        

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                path: "C:\\Users\\shian\\OneDrive\\Desktop\\pers\\.Net Core\\CartApi\\Logs\\log-.txt",
                outputTemplate: "{Timestamp: yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
                restrictedToMinimumLevel: LogEventLevel.Information,
                rollingInterval: RollingInterval.Day
            ).CreateLogger();

            services.AddLogging();

            //inject the global exception handling middleware
            services.AddIdentity<ApiUser, Role>()
                .AddEntityFrameworkStores<CartDbContext>();
            
            services.AddTransient<IGenericRepository<ApiUser>, GenericRepository<ApiUser>>();
            services.AddTransient<UserManager<ApiUser>>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<IAuthManager, AuthManager>();
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<IGenericRepository<CartApi.Domain.Entities.Cart>, GenericRepository<CartApi.Domain.Entities.Cart>>();
            services.AddTransient<IGenericRepository<Item>, GenericRepository<Item>>();
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                o.ReportApiVersions = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new MediaTypeApiVersionReader("ver"));
            });

            services.AddDbContext<CartDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("Default"));
            });

            

            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Cart Api", Version = "v1",  });
                
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    
                    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "Cart API V1");

                    //// Optionally, you can configure other Swagger UI options here:
                    //// For example, you can set the routePrefix to an empty string to achieve a custom URL structure
                    //c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //use global error handling
            app.UseMiddleware<GlobalExceptionHandler>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

       
        }
    }
}
