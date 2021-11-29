using Carting.DataAccess.LiteDb.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Carting.DataAccess.LiteDb
{
	public class LiteDbRepository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private bool _disposed;
		private readonly ILiteDatabase database;

		public LiteDbRepository(ILiteDbContext lightDbContext)
		{
			database = lightDbContext.Context;
		}

		public TEntity Create(TEntity item)
		{
			ILiteCollection<TEntity> collection = database.GetCollection<TEntity>();
			var id = collection.Insert(item);
			return collection.FindById(id);
		}

		public IEnumerable<TEntity> Get()
		{
			return database.GetCollection<TEntity>().FindAll();
		}

		public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
		{
			//Hack to include all fields of cart item
			return database.GetCollection<TEntity>().Include(x => (x as Cart).Items).Find(predicate);
		}

		public void Update(TEntity item)
		{
			database.GetCollection<TEntity>().Update(item);
		}

		public void Remove(int Id)
		{
			database.GetCollection<TEntity>().Delete(new BsonValue(Id));
		}

		public void RemoveItems(Expression<Func<TEntity, bool>> predicate)
		{
			database.GetCollection<TEntity>().DeleteMany(predicate);
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
						database.Dispose();
				}
				_disposed = true;
			}
		}

		~LiteDbRepository()
		{
			Dispose(false);
		}
	}
}
