
using Microsoft.EntityFrameworkCore;
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
        public async Task<Lead?> GetByIdForAiAsync(int id)
        {
            return await _context.Leads
                .Include(x => x.AssignedUser)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
