using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Service.Models
{
	public class CategoryModel
	{
		public int CategoryId { get; set; }

		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		public string Image { get; set; }

		public int? ParentCategoryId { get; set; }
	}
}
