using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.AuthDtos
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = null!;

        public string RefreshToken { get; set; } = null!;

        public DateTime ExpireDate { get; set; }
    }
}
