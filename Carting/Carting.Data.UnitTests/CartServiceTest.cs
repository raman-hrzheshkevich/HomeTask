using Carting.Data.Services;
using Carting.Data.Services.Models;
using Carting.DataAccess;
using Carting.DataAccess.LiteDb.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Carting.Data.UnitTests
{
	[TestClass]
	public class CartServiceTest
	{
		Mock<IRepository<CartItem>> cartItemRepositoryMock = new Mock<IRepository<CartItem>>();
		Mock<IRepository<Cart>> cartRepositoryMock = new Mock<IRepository<Cart>>();
		ICartService cartServiceIstance;
		private const string cartId = "cartId1";

		[TestInitialize]
		public void Init()
		{
			cartServiceIstance = new CartService(cartItemRepositoryMock.Object, cartRepositoryMock.Object);
			cartRepositoryMock.Setup(c => c.Get(It.IsAny<Expression<Func<Cart, bool>>>())).Returns(new List<Cart>()
			{
				new Cart()
			});
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "null argument is now allowed.")]
		public void AddItem_WhenCartItemIsNull_ShouldThrowArgumentNullException()
		{
			cartServiceIstance.AddItem(cartId, null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "empty name is not allowed.")]
		public void AddItem_WhenCartItemNameIsMissing_ShouldThrowArgumentException()
		{
			cartServiceIstance.AddItem(cartId, new CartItemModel
			{
				Id = 10,
				Price = 20
			});
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "empty price is now allowed.")]
		public void AddItem_WhenCartItemPriseIsMissing_ShouldThrowArgumentException()
		{
			cartServiceIstance.AddItem(cartId, new CartItemModel
			{
				Id = 10,
				Name = "Name"
			});
		}

		[TestMethod]
		public void AddItem_ShouldCallRepositoryCreate()
		{
			cartServiceIstance.AddItem(cartId, new CartItemModel
			{
				Id = 10,
				Name = "Name",
				Price = 20
			});


			cartItemRepositoryMock.Verify(o => o.Create(It.IsAny<CartItem>()), Times.Once);
		}
	}
}
