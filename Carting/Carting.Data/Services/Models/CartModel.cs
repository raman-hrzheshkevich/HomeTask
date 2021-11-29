using System.Collections.Generic;

namespace Carting.Data.Services.Models
{
	/// <summary>
	/// The cart model.
	/// </summary>
	public class CartModel
	{
		/// <summary>Gets the identifier.</summary>
		/// <value>The identifier.</value>
		public string Id { get; private set; }

		public CartModel(string Id)
		{
			this.Id = Id;
		}

		/// <summary>Gets or sets the cart items.</summary>
		/// <value>The cart items.</value>
		public IEnumerable<CartItemModel> CartItems { get; set; } = new List<CartItemModel>();
	}
}
