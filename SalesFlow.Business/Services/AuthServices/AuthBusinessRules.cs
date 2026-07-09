using Microsoft.AspNetCore.Identity;
using SalesFlow.Core.Exceptions;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.AuthServices
{
    public class AuthBusinessRules
    {
        private readonly UserManager<AppUser> _userManager;

        public AuthBusinessRules(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public void EnsureUserIsActive(AppUser user)
        {
            if (!user.IsActive)
                throw new BusinessException("User account is inactive.");
        }
    }
}
