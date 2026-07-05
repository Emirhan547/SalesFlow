using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.JwtDtos
{
    public class TokenUser
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public IList<string> Roles { get; set; } = new List<string>();
    }
}
