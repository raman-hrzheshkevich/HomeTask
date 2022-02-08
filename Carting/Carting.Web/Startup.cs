using Carting.DataAccess.LiteDb;
using Carting.Web.MessageBroker;
using MessageBroker;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;

namespace Carting.Web
{
	/// <summary>
	/// Startup class.
	/// </summary>
	public class Startup
	{
		/// <summary>Initializes a new instance of the <see cref="Startup" /> class.</summary>
		/// <param name="configuration">The configuration.</param>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>Gets the configuration.</summary>
		/// <value>The configuration.</value>
		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{

			services.AddControllers();

			services.AddSwaggerGen(setup =>
			{
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				setup.IncludeXmlComments(xmlPath);
			});

			services.ConfigureOptions<ConfigureSwaggerOptions>();

			services.AddApiVersioning(setup =>
			{
				setup.DefaultApiVersion = new ApiVersion(1, 0);
				setup.AssumeDefaultVersionWhenUnspecified = true;
				setup.ReportApiVersions = true;
			});

			services.AddVersionedApiExplorer(options =>
			{
				options.GroupNameFormat = "'v'VVV";
				options.SubstituteApiVersionInUrl = true;
			});

			services.AddCartingService(@"Filename=LiteDb.db;connection=shared");

			services.AddScoped<IMessageConsumer, MessageConsumer<ProductModel>>();
			services.AddScoped<IMessageSerializer, JsonMessageSerializer>();
			services.AddScoped<IMessageProcessor<ProductModel>, MessageProcessorService>();
			services.AddHostedService<WorkerMessageBroker>();
		}

		/// <summary>Configures the specified application.</summary>
		/// <param name="app">The application.</param>
		/// <param name="env">The env.</param>
		/// <param name="provider">The provider.</param>
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(options =>
				{
					// build a swagger endpoint for each discovered API version  
					foreach (var description in provider.ApiVersionDescriptions)
					{
						options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
					}
				});
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
