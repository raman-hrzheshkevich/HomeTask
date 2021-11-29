using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalog.DataAccess.Models
{
	public class Category
	{
		public int CategoryId { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public string Image { get; set; }

		[ForeignKey("Parent")]
		public int? ParentCategoryId { get; set; }

		public Category Parent { get; set; }

		public ICollection<Product> Products { get; set; }
	}
}
