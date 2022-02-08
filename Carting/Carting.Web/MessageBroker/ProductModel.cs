namespace Carting.Web.MessageBroker
{
	/// <summary>
	/// ProductModel class.
	/// </summary>
	public class ProductModel
	{
		/// <summary>Gets or sets the product identifier.</summary>
		/// <value>The product identifier.</value>
		public int ProductId { get; set; }

		/// <summary>Gets or sets the name.</summary>
		/// <value>The name.</value>
		public string Name { get; set; }

		/// <summary>Gets or sets the description.</summary>
		/// <value>The description.</value>
		public string Description { get; set; }

		/// <summary>Gets or sets the image.</summary>
		/// <value>The image.</value>
		public string Image { get; set; }

		/// <summary>Gets or sets the price.</summary>
		/// <value>The price.</value>
		public double Price { get; set; }

		/// <summary>Gets or sets the amount.</summary>
		/// <value>The amount.</value>
		public int Amount { get; set; }
	}
}
