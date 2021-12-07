using Catalog.Service;
using Catalog.Service.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Controllers
{
	[ApiController]
	[Route("v1/categories")]
	public class CategoryController : ControllerBase
	{
		private readonly ICategoryService catalogService;
		private readonly CategoryResource categoryResource;

		public CategoryController(ICategoryService catalogService, CategoryResource categoryResource)
		{
			this.catalogService = catalogService;
			this.categoryResource = categoryResource;
		}

		[HttpGet]
		public async Task<IActionResult> GetCategories()
		{
			IEnumerable<CategoryModel> result = await this.catalogService.GetCategories();

			return Ok(result.Select(categoryResource.Create));
		}

		[HttpDelete("{categoryId:long}")]
		public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
		{ 
			await catalogService.DeleteCategory(categoryId);

			return NoContent();
		}

		[HttpPut("{categoryId:long}")]
		public async Task<IActionResult> UpdateCategory([FromRoute] int categoryId, [FromBody] CategoryModel category)
		{
			await catalogService.UpdateCategory(categoryId, category);

			return NoContent();
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory([FromBody] CategoryModel category)
		{
			var result = await this.catalogService.AddCategory(category);

			return CreatedAtAction(nameof(GetCategories), categoryResource.Create(result));
		}
	}
}
