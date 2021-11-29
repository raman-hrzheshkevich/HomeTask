using LiteDB;
using System.Collections.Generic;

namespace Carting.DataAccess.LiteDb.Models
{
	public class Cart
	{
		static Cart() {
			BsonMapper.Global.Entity<Cart>()
				.Id(e => e.Id)
				.DbRef(x => x.Items, "CartItem");
	}
		public string Id { get; set; }

		public List<CartItem> Items { get; set; } = new List<CartItem>();
	}
}
