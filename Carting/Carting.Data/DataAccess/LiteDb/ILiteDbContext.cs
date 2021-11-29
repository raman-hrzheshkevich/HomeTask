using LiteDB;

namespace Carting.DataAccess.LiteDb
{
	public interface ILiteDbContext
	{
		ILiteDatabase Context { get; }
	}
}