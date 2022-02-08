using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Carting.Web
{
	/// <summary>
	///   ConfigureSwaggerOptions class.
	/// </summary>
	public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
	{
        private readonly IApiVersionDescriptionProvider provider;

		/// <summary>Initializes a new instance of the <see cref="ConfigureSwaggerOptions" /> class.</summary>
		/// <param name="provider">The provider.</param>
		public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }

		/// <summary>Invoked to configure a <span class="typeparameter">TOptions</span> instance.</summary>
		/// <param name="options">The options instance to configure.</param>
		public void Configure(SwaggerGenOptions options)
        {
            // add swagger document for every API version discovered
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }
        }

		/// <summary>Invoked to configure a <span class="typeparameter">TOptions</span> instance.</summary>
		/// <param name="name">The name of the options instance being configured.</param>
		/// <param name="options">The options instance to configure.</param>
		public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

		/// <summary>Creates the version information.</summary>
		/// <param name="description">The description.</param>
		/// <returns>
		///   <br />
		/// </returns>
		private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = "Carting API",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
