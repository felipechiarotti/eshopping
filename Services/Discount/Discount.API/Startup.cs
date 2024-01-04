using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Repositories;
using MediatR;

using System.Reflection;

namespace Discount.API
{
    public class Startup
    {
        public IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMediatR(typeof(CreateDiscountCommandHandler).GetTypeInfo().Assembly);
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.AddGrpc();

            //services.AddHealthChecks()
            //    .AddRedis(Configuration["CacheSettings:ConnectionString"], "Redis Health", Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Degraded);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DiscountService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with GRPC endpoints must be made through GRPC clients");
                });
            });
        }
    }
}
