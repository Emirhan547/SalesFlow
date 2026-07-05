using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.JwtDtos
{
    public class JwtSettings
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; set; } = null!;

        public string Audience { get; set; } = null!;

        public string SecretKey { get; set; } = null!;

        public int AccessTokenExpirationMinutes { get; set; }

        public int RefreshTokenExpirationDays { get; set; }
    }
}
