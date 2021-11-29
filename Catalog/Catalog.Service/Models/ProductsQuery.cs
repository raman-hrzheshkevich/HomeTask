namespace Catalog.Service.Models
{
	public class ProductsQuery
	{
		private const int DEFAULT_PAGE_NUMBER = 1;
		private const int DEFAULT_PAGE_SIZE = 10;

		public int? CategoryId { get; set; }

		public int PageNumber { get; set; } = DEFAULT_PAGE_NUMBER;

		public int PageSize { get; set; } = DEFAULT_PAGE_SIZE;
	}
}
