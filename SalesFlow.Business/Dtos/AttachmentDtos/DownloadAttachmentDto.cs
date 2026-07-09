using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.AttachmentDtos
{
    public class DownloadAttachmentDto
    {
        public byte[] FileBytes { get; set; } = null!;

        public string ContentType { get; set; } = null!;

        public string FileName { get; set; } = null!;
    }
}
