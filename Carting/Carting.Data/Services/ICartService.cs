using Carting.Data.Services.Models;
using System;
using System.Collections.Generic;

namespace Carting.Data.Services
{
	public interface ICartService : IDisposable
	{
		CartModel Create(string cartId);

		CartModel GetCart(string cartId);

		void AddItem(string cartId, CartItemModel cartItem);
		IEnumerable<CartItemModel> GetItems(string cartId);

		IEnumerable<CartItemModel> GetItems();

		void UpdateItem(CartItemModel itemModel);

		void RemoveItem(string cartId, int itemId);
	}
}