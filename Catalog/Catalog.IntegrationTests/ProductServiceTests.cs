using Catalog.DataAccess.Models;
using Catalog.Service;
using Catalog.Service.Models;
using Snapshooter.Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Catalog.IntegrationTests
{
	public class ProductServiceTests
	{
		[Fact]
		public async void AddProduct_ShouldAddProduct()
		{
			using (var ctx = new TestCategoryContext())
			{
				ProductService productService = new ProductService(ctx);

				await productService.AddProduct(new ProductModel()
				{
					CategoryId = 1,
					Name = "Product-new",
					Amount = 100,
					Price = 100

				});

				List<Product> result = ctx.Products
					.Where(c => c.Name == "Product-new")
					.ToList();

				Snapshot.Match(result);
			}
		}

		[Fact]
		public async void UpdateProduct_ShouldUpdateProduct()
		{
			using (var ctx = new TestCategoryContext())
			{
				ProductService productService = new ProductService(ctx);

				await productService.UpdateProduct(new ProductModel
				{
					CategoryId = 1,
					ProductId = 1,
					Name = "Changed name",
					Amount = 999,
					Price = 999
				});

				Product result = ctx.Products.Find(1);

				Snapshot.Match(result);
			}
		}

		[Fact]
		public async void DeleteProduct_ShouldDeleteProduct()
		{
			using (var ctx = new TestCategoryContext())
			{
				ProductService productService = new ProductService(ctx);

				await productService.DeleteProduct(2);

				var result = ctx.Categories.ToList();

				Snapshot.Match(result);
			}
		}

		[Fact]
		public async void GetProducts_ShouldGetAllProducts()
		{
			using (var ctx = new TestCategoryContext())
			{
				ProductService productService = new ProductService(ctx);

				var result = await productService.GetProducts(new ProductsQuery());

				Snapshot.Match(result);
			}
		}
	}
}
