﻿using Common.Logging.Correlation;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repository;
using Discount.Infrastructure.Repositories;
using MediatR;
using System.Reflection;

namespace Discount.API
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMediatR(typeof(CreateDiscountCommandHandler).GetTypeInfo().Assembly);
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddScoped<IDiscountRepository, DiscountRepository>();
            services.AddAutoMapper(typeof(Startup));
            services.AddGrpc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.AddCorrelationIdMiddleware();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<DiscountService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be through a gRPC client");
                });
            }); 
        }
    }
}
