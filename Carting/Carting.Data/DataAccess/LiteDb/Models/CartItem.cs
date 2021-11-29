using LiteDB;

namespace Carting.DataAccess.LiteDb.Models
{
	public class CartItem
	{

		[BsonId(true)]
		public int Id { get; set; }

		public string Name { get; set; }

		public CartImage Image { get; set; }

		public double Price { get; set; }

		public int Quantity { get; set; }
	}
}
