using Common.Logging.Correlation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocelot.ApiGateway
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();

            /*var authScheme = "EShoppingGatewayAuthScheme";
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(authScheme, opt =>
                {
                    opt.Authority = "https://localhost:9009";
                    opt.Audience = "EShoppingGateway";
                });*/

            services.AddOcelot()
                .AddCacheManager(o => o.WithDictionaryHandle());
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.AddCorrelationIdMiddleware();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello Ocelot");
                });
            });

            await app.UseOcelot();
        }
    }
}
