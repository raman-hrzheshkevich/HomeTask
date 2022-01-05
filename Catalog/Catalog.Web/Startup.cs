using Catalog.DataAccess;
using Catalog.Service;
using MessageBroker;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Catalog.Web
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddAutoMapper(Assembly.GetExecutingAssembly());
			services.AddSwaggerGen(opts =>
			{
				opts.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.Web", Version = "v1" });

				opts.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
				{
					Type = SecuritySchemeType.OAuth2,
					Flows = new OpenApiOAuthFlows()
					{
						Implicit = new OpenApiOAuthFlow()
						{
							Scopes = new Dictionary<string, string>
							{
								{ Configuration["AzureAd:Scope"] , "Access Application" },
							},
							AuthorizationUrl = new Uri($"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}/oauth2/v2.0/authorize"),
							TokenUrl = new Uri($"{Configuration["AzureAd:Instance"]}{Configuration["AzureAd:TenantId"]}/oauth2/v2.0/token"),
						},
					},
				});

				opts.AddSecurityRequirement(new OpenApiSecurityRequirement
					{
						{
							new OpenApiSecurityScheme
							{
								Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" },
								Scheme = "oauth2",
								Name = "oauth2",
								In = ParameterLocation.Header
							},
							new[] { Configuration["ApiScope"] }
						}
					});
			});

			services.AddAuthorization();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					Configuration.Bind("AzureAd", options);
					string tenantId = Configuration["AzureAd:TenantId"];
					string clientId = Configuration["AzureAd:ClientId"];
					string instanceId = Configuration["AzureAd:Instance"];
					options.Authority = $"{instanceId}{tenantId}/v2.0";
					
					options.TokenValidationParameters.ValidAudiences = new string[] { clientId, $"api://{clientId}" };
					options.TokenValidationParameters.ValidIssuers = new[]
					{
						$"https://sts.windows.net/{tenantId}/",
						$"{instanceId}{tenantId}/"
					};
				});

			services.AddScoped<ICategoryService, CategoryService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<CategoryResource>();
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddDbContext<CategoryContext>();

			services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

			services.AddScoped(provider =>
			{
				var actionContext = provider.GetRequiredService<IActionContextAccessor>().ActionContext;
				var factory = provider.GetRequiredService<IUrlHelperFactory>();
				return factory.GetUrlHelper(actionContext);
			});

			services.AddScoped<IMessageSender, MessageSender>();
			services.AddSingleton<IMessageSerializer, JsonMessageSerializer>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();

				app.UseSwaggerUI(opts =>
				{
					opts.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.Web v1");
					opts.OAuthClientId(Configuration["AzureAd:ClientId"]);
					opts.OAuthUsePkce();
				});
			}

			app.UseHttpsRedirection();

			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
