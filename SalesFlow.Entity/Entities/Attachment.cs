using SalesFlow.Entity.Common;


namespace SalesFlow.Entity.Entities
{
    public class Attachment : BaseEntity
    {
        public string FileName { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        public long FileSize { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; } = null!;
    }
}
