using Avon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Avon.Application.Repositories
{
	public interface IRepository<TEntity> where TEntity : class
	{

		Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes);
		Task<IQueryable<TEntity>> GetAsyncQuery(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes);
		Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes);
        Task<bool> IsAny(Expression<Func<TEntity, bool>> expression);


		Task InsertAsync(TEntity entity);
		Task InsertRangeAsync(List<TEntity> entities);
		
		Task Remove(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes);
        Task Remove(int id);
        Task RemoveRange(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes);


	}
}
