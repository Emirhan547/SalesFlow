using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.AttachmentDtos
{
    public class UpdateAttachmentDto
    {
        public int Id { get; set; }

        public string FileName { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        public long FileSize { get; set; }

        public int CustomerId { get; set; }
    }
}
