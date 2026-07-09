using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.FileServices
{
    public interface IFileService
    {
        Task<FileUploadResult> UploadAsync(IFormFile file);

        Task DeleteAsync(string filePath);
        Task<byte[]> ReadAsync(string filePath);
    }
}
