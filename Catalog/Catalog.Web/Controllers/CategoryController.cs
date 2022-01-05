using Catalog.Service;
using Catalog.Service.Models;
using Microsoft.AspNetCore.Authorization;
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
		[Authorize]
		public async Task<IActionResult> GetCategories()
		{
			IEnumerable<CategoryModel> result = await this.catalogService.GetCategories();

			return Ok(result.Select(categoryResource.Create));
		}

		[Authorize(Roles = "Task.Write")]
		[HttpDelete("{categoryId:long}")]
		public async Task<IActionResult> DeleteCategory([FromRoute] int categoryId)
		{ 
			await catalogService.DeleteCategory(categoryId);

			return NoContent();
		}

		[Authorize(Roles = "Task.Write")]
		[HttpPut("{categoryId:long}")]
		public async Task<IActionResult> UpdateCategory([FromRoute] int categoryId, [FromBody] CategoryModel category)
		{
			await catalogService.UpdateCategory(categoryId, category);

			return NoContent();
		}

		[Authorize(Roles = "Task.Write")]
		[HttpPost]
		public async Task<IActionResult> CreateCategory([FromBody] CategoryModel category)
		{
			var result = await this.catalogService.AddCategory(category);

			return CreatedAtAction(nameof(GetCategories), categoryResource.Create(result));
		}
	}
}
