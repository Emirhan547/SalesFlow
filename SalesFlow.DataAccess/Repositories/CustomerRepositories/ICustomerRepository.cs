using SalesFlow.DataAccess.Repositories.GenericRepositories;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.DataAccess.Repositories.CustomerRepositories
{
    public interface ICustomerRepository:IGenericRepository<Customer> 
    {
        Task<Customer?> GetByIdWithDetailsAsync(int id, bool tracking = false);
        Task<Customer?> GetCustomerWithTagsAsync(int customerId, bool tracking = false);
    }
}
