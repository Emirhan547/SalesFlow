using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.ProfileDtos
{
    public class GetProfileDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? PhoneNumber { get; set; }

        public string? ProfileImageUrl { get; set; }
    }
}
