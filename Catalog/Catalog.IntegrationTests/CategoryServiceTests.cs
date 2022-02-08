using Catalog.DataAccess;
using Catalog.DataAccess.Models;
using Catalog.Service;
using Catalog.Service.Models;
using Snapshooter.Xunit;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Xunit;

namespace Catalog.IntegrationTests
{
	[Collection("Sequential")]
	public class CategoryServiceTests
	{
		[Fact]
		public async void AddCategory_ShouldAddCategory()
		{
			using (var ctx = new TestCategoryContext())
			{
				CategoryService cs = new CategoryService(new GenericRepository<Category>(ctx));

				await cs.AddCategory(new CategoryModel()
				{
					Name = "Category-3",
				});

				List<Category> result = ctx.Categories.Where(c => c.Name == "Category-3")
					.Include(c => c.Products)
					.ToList();

				Snapshot.Match(result);
			}
		}

		[Fact]
		public async void UpdateCategory_ShouldUpdateCategory()
		{
			using (var ctx = new TestCategoryContext())
			{
				CategoryService cs = new  CategoryService(new GenericRepository<Category>(ctx));

				await cs.UpdateCategory(1, new CategoryModel { CategoryId = 1, Name = "Changed name" });

				Category result = ctx.Categories.Find(1);

				Snapshot.Match(result);
			}
		}

		[Fact]
		public async void DeleteCategory_ShouldDeleteCategory()
		{
			using (var ctx = new TestCategoryContext())
			{
				CategoryService cs = new  CategoryService(new GenericRepository<Category>(ctx));

				await cs.DeleteCategory(2);

				var result = ctx.Categories.ToList();

				Snapshot.Match(result);
			}
		}

		[Fact]
		public async void GetCategories_ShouldGetAllCategories()
		{
			using (var ctx = new TestCategoryContext())
			{
				CategoryService cs = new CategoryService(new GenericRepository<Category>(ctx));

				var result = await cs.GetCategories();

				Snapshot.Match(result);
			}
		}
	}
}
