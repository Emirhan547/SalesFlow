using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.FileServices
{
    public class FileUploadResult
    {
        public string FileName { get; set; } = null!;

        public string StoredFileName { get; set; } = null!;

        public string FilePath { get; set; } = null!;

        public long FileSize { get; set; }

        public string ContentType { get; set; } = null!;
    }
}
