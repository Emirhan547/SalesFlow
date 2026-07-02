using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Repositories.GenericRepositories;
using SalesFlow.Entity.Entities;


namespace SalesFlow.DataAccess.Repositories.DealRepositories;

    public class DealRepository : GenericRepository<Deal>, IDealRepository
    {
        public DealRepository(AppDbContext context) : base(context)
        {
        }
    }

