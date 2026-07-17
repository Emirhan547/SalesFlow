using Microsoft.AspNetCore.Identity;
using SalesFlow.Business.Dtos.ProfileDtos;
using SalesFlow.Core.Results;
using SalesFlow.Entity.Entities;

namespace SalesFlow.Business.Services.ProfileServices
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ProfileBusinessRules _businessRules;

        public ProfileService(
            UserManager<AppUser> userManager,
            ProfileBusinessRules businessRules)
        {
            _userManager = userManager;
            _businessRules = businessRules;
        }

        public async Task<Result<GetProfileDto>> GetProfileAsync(
            int userId)
        {
            AppUser user =
                await _businessRules
                    .GetUserByIdAsync(userId);

            GetProfileDto dto = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName!,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl
            };

            return Result<GetProfileDto>.Success(dto);
        }

        public async Task<Result> UpdateProfileAsync(
            int userId,
            UpdateProfileDto dto)
        {
            AppUser user =
                await _businessRules
                    .GetUserByIdAsync(userId);

            await _businessRules
                .EnsureUserNameIsAvailableAsync(
                    dto.UserName,
                    userId);

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.UserName = dto.UserName;
            user.PhoneNumber = dto.PhoneNumber;

            IdentityResult result =
                await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return Result.Failure(
                    result.Errors
                        .Select(x => x.Description)
                        .First());
            }

            return Result.Success(
                "Profile updated successfully.");
        }

        public async Task<Result> ChangePasswordAsync(
            int userId,
            ChangePasswordDto dto)
        {
            AppUser user =
                await _businessRules
                    .GetUserByIdAsync(userId);

            _businessRules.EnsurePasswordsMatch(
                dto.NewPassword,
                dto.ConfirmNewPassword);

            IdentityResult result =
                await _userManager.ChangePasswordAsync(
                    user,
                    dto.CurrentPassword,
                    dto.NewPassword);

            if (!result.Succeeded)
            {
                return Result.Failure(
                    result.Errors
                        .Select(x => x.Description)
                        .First());
            }

            return Result.Success(
                "Password changed successfully.");
        }
    }
}