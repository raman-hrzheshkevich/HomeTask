using Carting.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Carting.DataAccess.LiteDb
{
	public static class ServiceExtentions
	{
		public static void AddCartingService(this IServiceCollection services, string databasePath)
		{
			services.Configure<LiteDbOptions>(options => options.DatabaseLocation = databasePath);
			services.AddScoped<ILiteDbContext, LiteDbContext>();
			services.AddScoped(typeof(IRepository<>), typeof(LiteDbRepository<>));
			services.AddScoped<ICartService, CartService>();
		}
	}
}
