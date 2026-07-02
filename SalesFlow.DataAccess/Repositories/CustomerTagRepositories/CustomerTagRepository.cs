using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Repositories.GenericRepositories;
using SalesFlow.Entity.Entities;

namespace SalesFlow.DataAccess.Repositories.CustomerTagRepositories;

public class CustomerTagRepository : GenericRepository<CustomerTag>, ICustomerTagRepository
{
    public CustomerTagRepository(AppDbContext context) : base(context)
    {
    }
}

