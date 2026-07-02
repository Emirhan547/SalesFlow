using SalesFlow.Entity.Common;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace SalesFlow.DataAccess.Repositories.GenericRepositories
{
    public interface IGenericRepository<TEntity>where TEntity : BaseEntity
    {
        IQueryable<TEntity> GetAll(bool tracking = false);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity?> GetByIdAsync(int id, bool tracking = false);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);
    }
}
