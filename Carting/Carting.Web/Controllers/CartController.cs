using Carting.Data;
using Carting.Data.Services;
using Carting.Data.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Carting.Web.Controllers
{
	/// <summary>
	/// The Cart API Controller.
	/// </summary>
	[ApiVersion("1")]
	[ApiVersion("2")]
	[ApiController]
	[Route("v{version:apiVersion}/cart")]
	[Produces("application/json")]
	[Consumes("application/json")]
	public class CartController : ControllerBase
	{
		private readonly ICartService cartService;

		/// <summary>Initializes a new instance of the <see cref="CartController" /> class.</summary>
		/// <param name="cartService">The cart service.</param>
		public CartController(ICartService cartService)
		{
			this.cartService = cartService;
		}

		/// <summary>Gets the cart.</summary>
		/// <param name="cartId">The cart identifier.</param>
		/// <response code="200">The cart was retrieved successfully.</response>
		/// <response code="404">The cart was not found.</response>
		/// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
		/// <response code="500">A server fault occurred</response>
		[HttpGet("{cartId}")]
		[MapToApiVersion("1")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult GetCart(string cartId)
		{
			try
			{
				CartModel cart = this.cartService.GetCart(cartId);

				return Ok(cart);
			}

			catch (CartingException ex)
			{
				return NotFound(ex.Message);
			}
		}

		/// <summary>Adds item to the cart.</summary>
		/// <param name="cartId">The cart identifier.</param>
		/// <param name="model">The model.</param>
		/// <response code="200">Item was successfully added to the cart.</response>
		/// <response code="404">The cart was not found.</response>
		/// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
		/// <response code="500">A server fault occurred</response>
		[HttpPost("{cartId}")]
		[MapToApiVersion("1")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public IActionResult AddCartItem(string cartId, [FromBody] CartItemModel model)
		{
			try
			{
				this.cartService.AddItem(cartId, model);
				return Ok();
			}
			catch (CartingException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		/// <summary>Deletes the cart item.</summary>
		/// <param name="cartId">The cart identifier.</param>
		/// <param name="itemId">The item identifier.</param>
		/// <response code="200">Item was successfully deleted from the cart.</response>
		/// <response code="404">The cart was not found.</response>
		/// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
		/// <response code="500">A server fault occurred</response>
		[HttpDelete("{cartId}/items/{itemId}")]
		[MapToApiVersion("1")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public IActionResult DeleteCartItem(string cartId, int itemId)
		{
			try
			{
				this.cartService.RemoveItem(cartId, itemId);
				return Ok();
			}
			catch (CartingException ex)
			{
				return NotFound(ex.Message);
			}
		}

		/// <summary>Gets the cart items.</summary>
		/// <param name="cartId">The cart identifier.</param>
		/// <returns>
		/// List of cart items.
		/// </returns>
		/// <response code="200">List of cart items.</response>
		/// <response code="404">The cart was not found.</response>
		/// <response code="400">The request could not be understood by the server due to malformed syntax. The client SHOULD NOT repeat the request without modifications</response>
		/// <response code="500">A server fault occurred</response>
		[HttpGet("{cartId}")]
		[MapToApiVersion("2")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public IActionResult GetCartItems(string cartId)
		{
			try
			{
				IEnumerable<CartItemModel> cartItems = this.cartService.GetItems(cartId);

				return Ok(cartItems);
			}

			catch (CartingException ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
