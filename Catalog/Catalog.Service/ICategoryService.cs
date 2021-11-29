using Catalog.Service.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Service
{
	public interface ICategoryService
	{
		Task<CategoryModel> AddCategory(CategoryModel categoryModel);
		Task UpdateCategory(CategoryModel categoryModel);
		Task DeleteCategory(int categoryId);
		Task<IEnumerable<CategoryModel>> GetCategories();
	}
}