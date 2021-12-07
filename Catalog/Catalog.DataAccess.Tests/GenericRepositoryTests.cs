using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Catalog.DataAccess.Tests
{
	[TestClass]
	public class GenericRepositoryTests
	{
		private const string testCategoryId = "testCategoryId";

		private IGenericRepository<It.IsAnyType> repository;
		private Mock<CategoryContext> contextMock = new Mock<CategoryContext>();
		private Mock<DbSet<It.IsAnyType>> dbSetMock = new Mock<DbSet<It.IsAnyType>>();

		[TestInitialize]
		public void Initilize()
		{

			contextMock.Setup(c => c.Set<It.IsAnyType>()).Returns(dbSetMock.Object);
			repository = new GenericRepository<It.IsAnyType>(contextMock.Object);
		}

		[TestMethod]
		public void GetByID_ShouldCall_DBContentGet()
		{
			repository.GetByID(testCategoryId);

			dbSetMock.Verify(m => m.FindAsync(testCategoryId), Times.Once);
		}

		[TestMethod]
		public void Insert_ShouldCall_DBContentAdd()
		{
			repository.Insert(It.IsAny<It.IsAnyType>());

			dbSetMock.Verify(m => m.Add(It.IsAny<It.IsAnyType>()), Times.Once);
		}
	}
}
