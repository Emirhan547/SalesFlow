using Microsoft.EntityFrameworkCore;
using SalesFlow.DataAccess.Context;
using SalesFlow.Entity.Common;
using System.Linq.Expressions;


namespace SalesFlow.DataAccess.Repositories.GenericRepositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>where TEntity : BaseEntity
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> _table;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _table = _context.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll(bool tracking = false)
        {
            return tracking? _table: _table.AsNoTracking();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _table.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            _table.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            _table.Remove(entity);
        }
        public async Task<TEntity?> GetByIdAsync(int id, bool tracking = false)
        {
            var query = tracking? _table: _table.AsNoTracking();
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _table.AnyAsync(predicate);
        }
    }
}
