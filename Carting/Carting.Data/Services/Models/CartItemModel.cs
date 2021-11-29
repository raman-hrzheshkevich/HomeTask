using System.ComponentModel.DataAnnotations;

namespace Carting.Data.Services.Models
{
	/// <summary>
	/// The CartItemModel.
	/// </summary>
	public class CartItemModel
	{
		/// <summary>Gets or sets the identifier.</summary>
		/// <value>The identifier.</value>
		public int Id { get; set; }

		/// <summary>Gets or sets the name.</summary>
		/// <value>The name.</value>
		[Required]
		public string Name { get; set; }

		/// <summary>Gets or sets the image.</summary>
		/// <value>The image.</value>
		public CartImageModel Image { get; set; }

		/// <summary>Gets or sets the price.</summary>
		/// <value>The price.</value>
		[Required]
		[Range(double.Epsilon, double.MaxValue)]
		public double Price { get; set; }

		/// <summary>Gets or sets the quantity.</summary>
		/// <value>The quantity.</value>
		public int Quantity { get; set; }
	}
}
