using SalesFlow.DataAccess.Context;
using SalesFlow.DataAccess.Repositories.GenericRepositories;
using SalesFlow.Entity.Entities;

namespace SalesFlow.DataAccess.Repositories.AttachmentRepositories;

public class AttachmentRepository : GenericRepository<Attachment>, IAttachmentRepository
{
    public AttachmentRepository(AppDbContext context) : base(context)
    {
    }
}

