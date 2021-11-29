using Carting.DataAccess.LiteDb;
using LiteDB;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Carting.Data.UnitTests
{
	[TestClass]
	public class LiteDbRepositoryTest
	{
		Mock<ILiteDbContext> liteDbContextMock = new Mock<ILiteDbContext>();
		Mock<ILiteDatabase> liteDbMock = new Mock<ILiteDatabase>();
		Mock<ILiteCollection<It.IsAnyType>> liteCollectionMock = new Mock<ILiteCollection<It.IsAnyType>>();

		LiteDbRepository<It.IsAnyType> repositoryInstance;

		[TestInitialize]
		public void Init()
		{
			liteDbMock.Setup(o => o.GetCollection<It.IsAnyType>())
				.Returns(liteCollectionMock.Object);

			liteDbContextMock.Setup(o => o.Context).Returns(liteDbMock.Object);
			repositoryInstance = new LiteDbRepository<It.IsAnyType>(liteDbContextMock.Object);
		}

		[TestMethod]
		public void Create_ShouldCallDBInsert()
		{
			
			repositoryInstance.Create(new It.IsAnyType());

			liteCollectionMock.Verify(o => o.Insert(It.IsAny<It.IsAnyType>()), Times.Once);
		}

		[TestMethod]
		public void Remove_ShouldCallDBDelete()
		{
			int IdToDelete = 999;

			repositoryInstance.Remove(IdToDelete);

			liteCollectionMock.Verify(o => o.Delete(IdToDelete), Times.Once);
		}

		[TestMethod]
		public void Get_ShouldCallDBFindAll()
		{

			repositoryInstance.Get();

			liteCollectionMock.Verify(o => o.FindAll(), Times.Once);
		}
	}
}
