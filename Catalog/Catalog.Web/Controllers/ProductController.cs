using Catalog.Service;
using Catalog.Service.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Web.Controllers
{
	[ApiController]
	[Route("v1/products")]
	public class ProductController : ControllerBase
	{
		private readonly IProductService productService;

		public ProductController(IProductService productService)
		{
			this.productService = productService;
		}

		[Authorize()]
		[HttpGet]
		public async Task<IActionResult> GetProducts([FromQuery] ProductsQuery productsQuery)
		{
			var result = await this.productService.GetProducts(productsQuery);

			return Ok(result);
		}

		[HttpGet("properties/{productId:long}")]
		public async Task<IActionResult> GetProductProperties([FromRoute] int cartId)
		{
			var result = await Task.Run(() => new Dictionary<string, string>
			{
				{ $"testProp-1-{cartId}", $"testValue-1-{cartId}" },
				{ $"testProp-2-{cartId}", $"testValue-2-{cartId}" },
			});

			return Ok(result);
		}

		[Authorize(Roles = "Task.Write")]
		[HttpDelete("{productId:long}")]
		public async Task<IActionResult> DeleteProduct([FromRoute] int productId)
		{ 
			await productService.DeleteProduct(productId);

			return NoContent();
		}

		[Authorize(Roles = "Task.Write")]
		[HttpPut("{productId:long}")]
		public async Task<IActionResult> UpdateProduct([FromRoute] int productId, [FromBody] ProductModel product)
		{
			await productService.UpdateProduct(productId, product);

			return NoContent();
		}

		[Authorize(Roles = "Task.Write")]
		[HttpPost]
		public async Task<IActionResult> CreateProduct([FromBody] ProductModel product)
		{
			var result = await this.productService.AddProduct(product);

			return CreatedAtAction(nameof(GetProducts), result);
		}
	}
}
