using Microsoft.AspNetCore.Identity;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.AuthServices
{
    public class AuthBusinessRules
    {

        private readonly ICurrentUserService _currentUserService;

        public AuthBusinessRules(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public void EnsureUserIsActive(AppUser user)
        {
            if (!user.IsActive)
                throw new BusinessException("User account is inactive.");
        }
        public void EnsureCurrentUserCanAccess(int? userId)
        {
            if (_currentUserService.IsInRole("Admin"))
                return;

            if (_currentUserService.IsInRole("SalesManager"))
                return;

            if (_currentUserService.UserId is null)
                throw new ForbiddenException(
                    "User information could not be determined.");

            if (userId != _currentUserService.UserId)
            {
                throw new ForbiddenException(
                    "You are not authorized to access this resource.");
            }
        }
    }
}
