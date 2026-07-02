
using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Repositories.GenericRepositories;
using SalesFlow.Entity.Entities;

namespace SalesFlow.DataAccess.Repositories.LeadRepositories
{
    public class LeadRepository : GenericRepository<Lead>, ILeadRepository
    {
        public LeadRepository(AppDbContext context) : base(context)
        {
        }
    }
}
