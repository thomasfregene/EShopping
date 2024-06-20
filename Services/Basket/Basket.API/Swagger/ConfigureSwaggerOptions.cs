using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basket.API.Swagger
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var desc in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(desc.GroupName, ProvideApiInfo(desc));
            }
        }

        private OpenApiInfo ProvideApiInfo(ApiVersionDescription desc)
        {
            var info = new OpenApiInfo
            {
                Title = "Basket API Microservice",
                Version = desc.ApiVersion.ToString(),
                Description = "Fetches details about Basket",
                Contact = new OpenApiContact() { Name = "Fregene Thomas", Email = "fregzy@email.com" },
                License = new OpenApiLicense() { Name = "MIT", Url = new Uri("hhtps://opensource.org/licenses/MIT") }
            };
            if (desc.IsDeprecated)
            {
                info.Description += "API version has been deprecated!!!";
            }
            return info;
        }
    }
}
