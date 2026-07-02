
namespace SalesFlow.DataAccess.Uows
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
