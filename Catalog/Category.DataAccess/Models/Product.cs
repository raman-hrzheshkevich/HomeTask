using System.ComponentModel.DataAnnotations;

namespace Catalog.DataAccess.Models
{
	public class Product
	{
		public int ProductId { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public int CategoryId { get; set; }

		public string Description { get; set; }

		public string Image { get; set; }

		[Required]
		public decimal Price { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int Amount { get; set; }

		//[ForeignKey("CategoryId")]
		public Category Category { get; set; }

	}
}
