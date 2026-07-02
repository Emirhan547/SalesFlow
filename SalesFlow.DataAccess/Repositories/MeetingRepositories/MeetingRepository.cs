using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Repositories.GenericRepositories;
using SalesFlow.Entity.Entities;


namespace SalesFlow.DataAccess.Repositories.MeetingRepositories
{
    public class MeetingRepository : GenericRepository<Meeting>, IMeetingRepository
    {
        public MeetingRepository(AppDbContext context) : base(context)
        {
        }
    }
}
