using Carting.Data.Services.Models;
using Carting.DataAccess;
using Carting.DataAccess.LiteDb.Models;
using Dawn;
using LiteDB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;

namespace Carting.Data.Services
{
	public class CartService : ICartService, IDisposable
	{
		private bool _disposed;
		private readonly IRepository<CartItem> repository;
		private readonly IRepository<Cart> cartRepository;

		public CartService(IRepository<CartItem> repository, IRepository<Cart> cartRepository)
		{
			this.repository = repository;
			this.cartRepository = cartRepository;
		}

		public CartModel Create(string id)
		{
			Cart result = this.cartRepository.Create(new Cart() { Id = id });
			return AutoMapper.Map<CartModel>(result);
		}

		public void AddItem(string cartId, CartItemModel cartItem)
		{
			Guard.Argument(cartItem, nameof(cartItem)).NotNull();
			if (!this.ValidateCartId(cartId))
			{
				this.Create(cartId);
			}

			this.Validate(cartItem);

			CartItem newItem = AutoMapper.Map<CartItem>(cartItem);
			this.repository.Create(newItem);

			Cart cart = this.cartRepository.Get(c => c.Id == cartId).Single();

			cart.Items.Add(newItem);

			this.cartRepository.Update(cart);
		}

		public CartModel GetCart(string cartId)
		{
			if (!this.ValidateCartId(cartId))
			{
				throw new CartingException($"No cart was found for cartId [${cartId}]");
			}

			var cart = this.cartRepository.Get(c => c.Id == cartId).FirstOrDefault();
			return AutoMapper.Map<CartModel>(cart);
		}

		public IEnumerable<CartItemModel> GetItems(string cartId)
		{
			if (!this.ValidateCartId(cartId))
			{
				throw new CartingException($"No cart was found for cartId [${cartId}]");
			}

			return this.GetCart(cartId).CartItems;
		}

		public void RemoveItem(string cartId, int itemId)
		{
			Guard.Argument(itemId, nameof(itemId)).NotNegative();
			if (!this.cartRepository.Get(c => c.Id == cartId).Any())
			{
				throw new CartingException($"No cart was found for cartId [${cartId}]");
			}


			var cartItem = this.repository.Get(i => i.Id == itemId).FirstOrDefault();
			this.repository.Remove(itemId);

			Cart cart = this.cartRepository.Get(c => c.Id == cartId).Single();
			cart.Items.Remove(cartItem);
		}

		private void Validate(CartItemModel cartItem)
		{
			var valiadtionContext = new ValidationContext(cartItem);
			var results = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(cartItem, valiadtionContext, results, true);

			if (!isValid)
			{
				string errorMessages = string.Join(";",results.Select(r => r.ErrorMessage));
				throw new ArgumentException($"Model is not valid: ${errorMessages}");
			}
		}

		private bool ValidateCartId(string cartId)
		{
			return this.cartRepository.Get(c => c.Id == cartId).Any();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					this.repository.Dispose();
				}
				_disposed = true;
			}
		}

		~CartService()
		{
			Dispose(false);
		}
	}
}
