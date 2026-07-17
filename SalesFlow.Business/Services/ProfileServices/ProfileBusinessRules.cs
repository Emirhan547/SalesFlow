using Microsoft.AspNetCore.Identity;
using SalesFlow.Core.Exceptions;
using SalesFlow.Entity.Entities;

namespace SalesFlow.Business.Services.ProfileServices
{
    public class ProfileBusinessRules
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileBusinessRules(
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser> GetUserByIdAsync(
            int userId)
        {
            AppUser? user =
                await _userManager.FindByIdAsync(
                    userId.ToString());

            if (user is null)
                throw new NotFoundException(
                    "User not found.");

            return user;
        }

        public async Task EnsureUserNameIsAvailableAsync(
            string userName,
            int userId)
        {
            AppUser? existingUser =
                await _userManager.FindByNameAsync(
                    userName);

            if (
                existingUser is not null &&
                existingUser.Id != userId
            )
            {
                throw new BusinessException(
                    "Username is already in use.");
            }
        }

        public void EnsurePasswordsMatch(
            string newPassword,
            string confirmNewPassword)
        {
            if (newPassword != confirmNewPassword)
            {
                throw new BusinessException(
                    "New passwords do not match.");
            }
        }
    }
}