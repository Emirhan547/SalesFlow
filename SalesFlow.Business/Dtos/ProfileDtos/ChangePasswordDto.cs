using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Dtos.ProfileDtos
{
    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = null!;

        public string NewPassword { get; set; } = null!;

        public string ConfirmNewPassword { get; set; } = null!;
    }
}
