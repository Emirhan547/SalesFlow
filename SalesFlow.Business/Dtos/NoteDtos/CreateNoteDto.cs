using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.NoteDtos
{
    public class CreateNoteDto
    {
        public string Content { get; set; } = null!;

        public int CustomerId { get; set; }

        public int? CreatedById { get; set; }
    }
}
