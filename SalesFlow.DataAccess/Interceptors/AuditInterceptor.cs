using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SalesFlow.Entity.Common;


namespace SalesFlow.DataAccess.Interceptors
{
    public class AuditInterceptor : SaveChangesInterceptor
    {
       
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            ApplyAuditInformation(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private static void ApplyAuditInformation(DbContext? context)
        {
            if (context is null)
                return;

            var entries = context.ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:

                        entry.Entity.CreatedDate = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;

                        break;

                    case EntityState.Modified:

                        entry.Entity.UpdatedDate = DateTime.UtcNow;

                        break;

                    case EntityState.Deleted:

                        entry.State = EntityState.Modified;

                        entry.Entity.IsDeleted = true;
                        entry.Entity.UpdatedDate = DateTime.UtcNow;

                        break;
                }
            }
        }
    }
}