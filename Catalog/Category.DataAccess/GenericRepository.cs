using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Catalog.DataAccess
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{
		internal CategoryContext context;
		internal DbSet<TEntity> dbSet;

		public GenericRepository(CategoryContext context)
		{
			this.context = context;
			this.dbSet = context.Set<TEntity>();
		}

		public async virtual Task<ICollection<TEntity>> Get(
			Expression<Func<TEntity, bool>> filter = null)
		{
			IQueryable<TEntity> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.ToListAsync();
		}

		public async virtual Task<TEntity> GetByID(object id)
		{
			return await dbSet.FindAsync(id);
		}

		public async virtual Task<TEntity> Insert(TEntity entity) 
		{
			dbSet.Add(entity);
			await this.context.SaveChangesAsync().ConfigureAwait(false);

			return entity;
		}

		public async virtual Task Delete(object id)
		{
			TEntity entityToDelete = await dbSet.FindAsync(id);
			dbSet.Remove(entityToDelete);

			await context.SaveChangesAsync().ConfigureAwait(false);
		}

		public async virtual Task Update(TEntity entityToUpdate)
		{
			await dbSet.AddAsync(entityToUpdate);
			context.Entry(entityToUpdate).State = EntityState.Modified;

			await context.SaveChangesAsync().ConfigureAwait(false);
		}
	}
}
