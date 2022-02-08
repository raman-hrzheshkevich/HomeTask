using Carting.Data.Services;
using MessageBroker;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Carting.Web.MessageBroker
{
	/// <summary>
	///   MessageProcessorService class.
	/// </summary>
	public class MessageProcessorService : IMessageProcessor<ProductModel>
	{
		private readonly ICartService cartService;

		/// <summary>Initializes a new instance of the <see cref="MessageProcessorService" /> class.</summary>
		/// <param name="cartService">The cart service.</param>
		public MessageProcessorService(ICartService cartService)
		{
			this.cartService = cartService;
		}

		/// <summary>Processes the message.</summary>
		/// <param name="message">The message.</param>
		/// <returns>
		///   <br />
		/// </returns>
		public Task ProcessMessage(ProductModel message)
		{

			var cartItems = this.cartService.GetItems();

			var itemToUpdate = cartItems.FirstOrDefault(i => i.Id == message.ProductId);

			if (itemToUpdate != null)
			{
				itemToUpdate.Image.Url = message.Image;
				itemToUpdate.Price = message.Price;
				itemToUpdate.Quantity = message.Amount;
				itemToUpdate.Name = message.Name;

				this.cartService.UpdateItem(itemToUpdate);
			}

			return Task.CompletedTask;
		}
	}
}
