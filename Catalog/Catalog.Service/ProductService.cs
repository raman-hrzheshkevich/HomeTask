using Catalog.DataAccess;
using Catalog.DataAccess.Models;
using Catalog.Service.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.Service
{
	public class ProductService : IProductService
	{
		private readonly IGenericRepository<Product> productRepository;

		public ProductService(IGenericRepository<Product> productRepository)
		{
			this.productRepository = productRepository;
		}

		public async Task<ProductModel> AddProduct(ProductModel productModel)
		{
			var product = AutoMapper.Map<Product>(productModel);

			var result = await productRepository.Insert(product);

			return AutoMapper.Map<ProductModel>(result);
		}

		public async Task UpdateProduct(int productId, ProductModel productModel)
		{
			var product = AutoMapper.Map<Product>(productModel);
			product.ProductId = productId;

			await productRepository.Update(product);
		}

		public async Task DeleteProduct(int productId)
		{
			await productRepository.Delete(productId);
		}

		public async Task<IEnumerable<ProductModel>> GetProducts(ProductsQuery query)
		{
			Expression<Func<Product, bool>> filterExpression = (p) => query.CategoryId == null || p.CategoryId == query.CategoryId;

			ICollection<Product> result = await productRepository.Get(filterExpression);

			return result
					.Skip(query.PageSize * (query.PageNumber - 1))
					.Take(query.PageSize)
					.Select(r => AutoMapper.Map<ProductModel>(r));
		}
	}
}
