using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.ExportServices
{
    public interface IPdfExportService
    {
        byte[] ExportCustomers(IEnumerable<Customer> customers);

        byte[] ExportLeads(IEnumerable<Lead> leads);

        byte[] ExportDeals(IEnumerable<Deal> deals);
    }
}
