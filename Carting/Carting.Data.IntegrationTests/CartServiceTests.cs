using Carting.Data.Services;
using Carting.Data.Services.Models;
using Carting.DataAccess.LiteDb;
using Carting.DataAccess.LiteDb.Models;
using LiteDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Carting.Data.IntegrationTests
{
	[TestClass]
	public class CartServiceTests
	{
		private ICartService cartServiceInstance;
		private const string connectionString = "LiteDb.db";
		private const string testCartId = "cartId1";

		[TestInitialize]

		public void Initialize()
		{
			using (LiteDatabase database = new LiteDatabase(connectionString))
			{
				ILiteCollection<Cart> cartCollection = database.GetCollection<Cart>();
				ILiteCollection<CartItem> cartItemCollection = database.GetCollection<CartItem>();



				var cartItem1 = new CartItem()
				{
					Name = "item1",
					Price = 1,
					Quantity = 1
				};

				var cartItem2 = new CartItem()
				{
					Name = "item2",
					Price = 2,
					Quantity = 2,
				};

				cartItemCollection.Insert(cartItem1);
				cartItemCollection.Insert(cartItem2);

				cartCollection.Insert(new Cart()
				{
					Id = testCartId,
					Items = new List<CartItem>()
					{
						cartItem1,
						cartItem2
					}
				});
			}

			LiteDbContext context = new LiteDbContext(connectionString);

			cartServiceInstance = new CartService(
				new LiteDbRepository<CartItem>(context),
				new LiteDbRepository<Cart>(context));
		}

		[TestMethod]
		public void AddItem_ShouldAddItemToLiteDb()
		{
			using (cartServiceInstance)
			{
				CartModel cart = cartServiceInstance.Create("cart");

				cartServiceInstance.AddItem(cart.Id, new CartItemModel()
				{
					Name = "item3",
					Price = 20,
					Quantity = 30,
				});

				cartServiceInstance.AddItem(cart.Id, new CartItemModel()
				{
					Name = "item4",
					Price = 2,
					Quantity = 3,
					Image = new CartImageModel
					{
						AltText = "sample image",
						Url = "sampleUrl.jpg"
					}
				});
			}


			using (LiteDatabase database = new LiteDatabase(connectionString))
			{
				CartItem newItem1 = database.GetCollection<CartItem>().FindOne(c => c.Name == "item3");
				CartItem newItem2 = database.GetCollection<CartItem>().FindOne(c => c.Name == "item4");

				Assert.IsNotNull(newItem1);
				Assert.AreEqual("item3", newItem1.Name);
				Assert.AreEqual(20, newItem1.Price);
				Assert.AreEqual(30, newItem1.Quantity);

				Assert.IsNotNull(newItem2);
				Assert.AreEqual("item4", newItem2.Name);
				Assert.AreEqual(2, newItem2.Price);
				Assert.AreEqual(3, newItem2.Quantity);
				Assert.IsNotNull(newItem2.Image);
				Assert.AreEqual("sample image", newItem2.Image.AltText);
				Assert.AreEqual("sampleUrl.jpg", newItem2.Image.Url);
			}
		}

		[TestMethod]
		public void RemoveItem_ShouldRemoveItemFromLiteDb()
		{
			using (cartServiceInstance)
			{
				cartServiceInstance.RemoveItem(testCartId, 1);
			}

			using (LiteDatabase database = new LiteDatabase(connectionString))
			{
				CartItem item = database.GetCollection<CartItem>().FindOne(c => c.Name == "item1");

				Assert.IsNull(item);
			}
		}

		[TestMethod]
		public void GetItems_ShouldReturnListOfItemsFromLiteDb()
		{
			using (cartServiceInstance)
			{
				IEnumerable<CartItemModel> result = cartServiceInstance.GetItems(testCartId).ToList();

				Assert.IsNotNull(result);
				Assert.AreEqual(2, result.Count());

				CartItemModel firstItem = result.ElementAt(0);
				Assert.AreEqual("item1", firstItem.Name);
				Assert.AreEqual(1, firstItem.Price);
				Assert.AreEqual(1, firstItem.Quantity);

				CartItemModel secondItem = result.ElementAt(1);
				Assert.AreEqual("item2", secondItem.Name);
				Assert.AreEqual(2, secondItem.Price);
				Assert.AreEqual(2, secondItem.Quantity);
			}
		}

		[TestCleanup]
		public void CleanUp()
		{
			File.Delete(connectionString);
		}
	}
}
