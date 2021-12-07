using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.DataAccess
{
	public interface IGenericRepository<TEntity> where TEntity : class
	{
		Task Delete(object id);
		Task<ICollection<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null);
		Task<TEntity> GetByID(object id);
		Task<TEntity> Insert(TEntity entity);
		Task Update(TEntity entityToUpdate);
	}
}