using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.AttachmentDtos
{
    public class CreateAttachmentDto
    {
        public IFormFile File { get; set; } = null!;

        public int CustomerId { get; set; }
    }
}
