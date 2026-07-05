using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.TagDtos
{
    public class CreateTagDto
    {
        public string Name { get; set; } = null!;

        public string? Color { get; set; }
    }
}
