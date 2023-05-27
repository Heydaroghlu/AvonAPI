
using Avon.Application.Repositories;
using Avon.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Avon.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{

		readonly DataContext _context;
		public Repository(DataContext context)
		{
			_context = context;
		}

		DbSet<TEntity> Table
		{
			get => _context.Set<TEntity>();
			set => _context.Set<TEntity>();
		}

		public async Task<IQueryable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes)
		{
			var query = Table.Where(expression);
		 	query = IsTracking(query, tracking);
            query = Includes(query,includes);
			return query;
		}

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes)
		{
			var query = Table.AsQueryable<TEntity>();
			query = IsTracking(query, tracking);
            query = Includes(query, includes);


			return await query.FirstOrDefaultAsync(expression);
		}

		public async Task<IQueryable<TEntity>> GetAsyncQuery(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes)
		{
			var query = Table.AsQueryable<TEntity>().Where(expression);
			query = IsTracking(query, tracking);
			query = Includes(query, includes);


			return query;
		}

		public async Task InsertAsync(TEntity entity)
		{
			await Table.AddAsync(entity);
		}

		public async Task InsertRangeAsync(List<TEntity> entities)
		{
			await Table.AddRangeAsync(entities);
		}

		public async Task<bool> IsAny(Expression<Func<TEntity, bool>> expression)
		{
		    return await Table.AnyAsync(expression);
		}

		public async Task Remove(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes)
		{
			var entity = await Table.FirstOrDefaultAsync(expression);
			Table.Remove(entity);
		}
        public async Task Remove(int id)
        {
            var entity = await Table.FindAsync(id);
            Table.Remove(entity);
        }

        public async Task RemoveRange(Expression<Func<TEntity, bool>> expression, bool tracking = true, params string[] includes)
		{
			var entities = Table.Where(expression).ToList();
			Table.RemoveRange(entities);
		}

		private IQueryable<TEntity> IsTracking(IQueryable<TEntity> query, bool tracking)
		{
			if (!tracking)
			{
				query = query.AsNoTracking();
			}
			return query;

		}
		private IQueryable<TEntity> Includes(IQueryable<TEntity> query, params string[] includes)
		{
			foreach (var include in includes)
			{
				query = query.Include(include);
			}
            return query;
        }

    }
}
