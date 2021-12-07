using Catalog.DataAccess;
using Catalog.DataAccess.Models;
using Catalog.Service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Service
{
	public class CategoryService : ICategoryService
	{
		private readonly IGenericRepository<Category> categoryRepository;

		public CategoryService(IGenericRepository<Category> categoryRepository)
		{
			this.categoryRepository = categoryRepository;
		}

		public async Task<CategoryModel> AddCategory(CategoryModel categoryModel)
		{
			var category = AutoMapper.Map<Category>(categoryModel);

			var result = await categoryRepository.Insert(category);

			return AutoMapper.Map<CategoryModel>(result);
		}

		public async Task UpdateCategory(int categoryId, CategoryModel categoryModel)
		{
			var category = AutoMapper.Map<Category>(categoryModel);
			category.CategoryId = categoryId;

			await categoryRepository.Update(category);
		}

		public async Task DeleteCategory(int categoryId)
		{
			await categoryRepository.Delete(categoryId);
		}

		public async Task<IEnumerable<CategoryModel>> GetCategories()
		{
			var result = await categoryRepository.Get();

			return result.Select(r => AutoMapper.Map<CategoryModel>(r));
		}
	}
}
