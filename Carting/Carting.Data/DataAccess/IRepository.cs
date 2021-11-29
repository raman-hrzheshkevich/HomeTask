using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Carting.DataAccess
{
	public interface IRepository<TEntity> : IDisposable where TEntity : class
    {
        TEntity Create(TEntity item);
        IEnumerable<TEntity> Get();
        void Update(TEntity item);
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        void Remove(int Id);

        void RemoveItems(Expression<Func<TEntity, bool>> predicate);
    }
}
