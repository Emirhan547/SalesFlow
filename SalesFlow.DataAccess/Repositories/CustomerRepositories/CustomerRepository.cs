using Microsoft.EntityFrameworkCore;
using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Repositories.GenericRepositories;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Repositories.CustomerRepositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Customer?> GetByIdWithDetailsAsync(int id, bool tracking = false)
        {
            IQueryable<Customer> query = tracking? _table: _table.AsNoTracking();
            return await query.Include(x => x.Deals).Include(x => x.Meetings).Include(x => x.Notes).Include(x => x.Attachments) .Include(x => x.CustomerTags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<Customer?> GetCustomerWithTagsAsync(int customerId, bool tracking = false)
        {
            IQueryable<Customer> query = _table
                .Include(x => x.CustomerTags)
                .ThenInclude(x => x.Tag);

            if (!tracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == customerId);
        }
        public async Task<Customer?> GetByIdForAiInsightsAsync(int id)
        {
            return await _context.Customers
                .Include(x => x.AssignedUser)
                .Include(x => x.Deals)
                .Include(x => x.Meetings)
                .Include(x => x.Notes)
                .Include(x => x.TaskItems)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
