using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.TagDtos
{
    public class ResultTagDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? Color { get; set; }
    }
}
