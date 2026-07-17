using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.UserDtos
{
    public class ResultUserDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UserName { get; set; } = null!;
    }
}
