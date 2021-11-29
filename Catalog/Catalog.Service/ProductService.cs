using Catalog.DataAccess;
using Catalog.DataAccess.Models;
using Catalog.Service.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Service
{
	public class ProductService : IProductService
	{
		private readonly CategoryContext context;

		public ProductService(CategoryContext context)
		{
			this.context = context;
		}

		public async Task<ProductModel> AddProduct(ProductModel productModel)
		{
			var product = AutoMapper.Map<Product>(productModel);
			await context.Products.AddAsync(product);

			await context.SaveChangesAsync().ConfigureAwait(false);

			var result = await context.Products
				.FirstOrDefaultAsync(p => p.ProductId == product.ProductId)
				.ConfigureAwait(false);

			return AutoMapper.Map<ProductModel>(result);
		}

		public async Task UpdateProduct(ProductModel productModel)
		{
			var product = AutoMapper.Map<Product>(productModel);
			context.Products.Update(product);

			await context.SaveChangesAsync().ConfigureAwait(false);
		}

		public async Task DeleteProduct(int productId)
		{
			var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
			context.Products.Remove(product);

			await context.SaveChangesAsync().ConfigureAwait(false);
		}

		public async Task<IEnumerable<ProductModel>> GetProducts(ProductsQuery query)
		{
			var result = await context.Products
				.Where(p => query.CategoryId == null || p.CategoryId == query.CategoryId)
				.Skip(query.PageSize * (query.PageNumber - 1))
				.Take(query.PageSize)
				.ToListAsync()
				.ConfigureAwait(false);

			return result.Select(r => AutoMapper.Map<ProductModel>(r));
		}
	}
}
