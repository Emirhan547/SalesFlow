using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SalesFlow.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.FileServices
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _environment;

        private const long MaxFileSize = 10 * 1024 * 1024; //10 MB

        private readonly string[] AllowedExtensions =
        [
            ".jpg",
        ".jpeg",
        ".png",
        ".pdf",
        ".doc",
        ".docx",
        ".xls",
        ".xlsx"
        ];

        public FileService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<FileUploadResult> UploadAsync(IFormFile file)
        {
            ValidateFile(file);

            string uploadsFolder = Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "attachments");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string extension = Path.GetExtension(file.FileName);

            string storedFileName =
                $"{Guid.NewGuid()}{extension}";

            string fullPath = Path.Combine(
                uploadsFolder,
                storedFileName);

            await using FileStream stream =
                new(fullPath, FileMode.Create);

            await file.CopyToAsync(stream);

            return new FileUploadResult
            {
                FileName = file.FileName,
                StoredFileName = storedFileName,
                FilePath = $"uploads/attachments/{storedFileName}",
                FileSize = file.Length,
                ContentType = file.ContentType
            };
        }

        public Task DeleteAsync(string filePath)
        {
            string fullPath = Path.Combine(
                _environment.WebRootPath,
                filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }

        private void ValidateFile(IFormFile file)
        {
            if (file is null || file.Length == 0)
                throw new BusinessException("File is required.");

            if (file.Length > MaxFileSize)
                throw new BusinessException("Maximum file size is 10 MB.");

            string extension =
                Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!AllowedExtensions.Contains(extension))
                throw new BusinessException("File type is not supported.");
        }
        public async Task<byte[]> ReadAsync(string filePath)
        {
            string fullPath = Path.Combine(
                _environment.WebRootPath,
                filePath);

            if (!File.Exists(fullPath))
                throw new NotFoundException("File not found.");

            return await File.ReadAllBytesAsync(fullPath);
        }
    }
}