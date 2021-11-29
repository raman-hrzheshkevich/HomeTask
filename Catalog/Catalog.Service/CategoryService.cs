using Catalog.DataAccess;
using Catalog.DataAccess.Models;
using Catalog.Service.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Service
{
	public class CategoryService : ICategoryService
	{
		private readonly CategoryContext context;

		public CategoryService(CategoryContext context)
		{
			this.context = context;
		}

		public async Task<CategoryModel> AddCategory(CategoryModel categoryModel)
		{
			var category = AutoMapper.Map<Category>(categoryModel);
			await context.Categories.AddAsync(category);

			await context.SaveChangesAsync().ConfigureAwait(false);

			var result = await context.Categories
				.FirstOrDefaultAsync(c => c.CategoryId  == category.CategoryId)
				.ConfigureAwait(false);

			return AutoMapper.Map<CategoryModel>(result);
		}

		public async Task UpdateCategory(CategoryModel categoryModel)
		{
			var category = AutoMapper.Map<Category>(categoryModel);
			context.Categories.Update(category);

			await context.SaveChangesAsync().ConfigureAwait(false);
		}

		public async Task DeleteCategory(int categoryId)
		{
			var category = await context.Categories.FirstOrDefaultAsync(p => p.CategoryId == categoryId);
			context.Categories.Remove(category);
			var products = context.Products.Where(p => p.CategoryId == categoryId);
			context.Products.RemoveRange(products);

			await context.SaveChangesAsync().ConfigureAwait(false);
		}

		public async Task<IEnumerable<CategoryModel>> GetCategories()
		{
			var result = await context.Categories
				.ToListAsync()
				.ConfigureAwait(false);

			return result.Select(r => AutoMapper.Map<CategoryModel>(r));
		}
	}
}
