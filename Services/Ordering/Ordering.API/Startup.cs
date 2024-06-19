using Common.Logging.Correlation;
using EventBus.Messges.Common;
using HealthChecks.UI.Client;
using MassTransit;
using Ordering.API.EventBusConsumer;
using Ordering.Application.Extensions;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Extensions;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddApiVersioning();
            services.AddApplicationServices();
            services.AddInfraServices(Configuration);
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<BasketOrderingConsumer>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Ordering.API",
                    Version = "v1"
                });
            });
            services.AddHealthChecks().Services.AddDbContext<OrderContext>();

            services.AddMassTransit(config =>
            {
                config.AddConsumer<BasketOrderingConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(Configuration["EventBusSettings:HostAddress"]);
                    cfg.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
                    {
                        c.ConfigureConsumer<BasketOrderingConsumer>(ctx);
                    });
                });
            });

            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ordering.API v1"));
            }

            app.UseHttpsRedirection();
            //app.AddCorrelationIdMiddleware();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
