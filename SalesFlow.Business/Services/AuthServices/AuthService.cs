using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesFlow.Business.Dtos.AuthDtos;
using SalesFlow.Business.Dtos.JwtDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.JwtServices;
using SalesFlow.Core.Constants;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthBusinessRules _businessRules;
        private readonly ITokenService _tokenService;
        private readonly IActivityLogService _activityLogService;
        private readonly IUnitOfWork _unitOfWork;
        public AuthService(UserManager<AppUser> userManager, AuthBusinessRules businessRules, ITokenService tokenService, IActivityLogService activityLogService, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _businessRules = businessRules;
            _tokenService = tokenService;
            _activityLogService = activityLogService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            AppUser? user = await _userManager.FindByEmailAsync(dto.Email);

            if (user is null)
                return Result<LoginResponseDto>.Failure("Invalid email or password.");

            _businessRules.EnsureUserIsActive(user);

            bool checkPassword = await _userManager.CheckPasswordAsync(user, dto.Password);

            if (!checkPassword)
                return Result<LoginResponseDto>.Failure("Invalid email or password.");

            LoginResponseDto response = await BuildLoginResponseAsync(user);
            await _activityLogService.AddAsync(ActivityAction.Login,nameof(AppUser),user.Id,$"User '{user.UserName}' logged in.",user.Id);

           
            return Result<LoginResponseDto>.Success(response);
        }

        public async Task<Result> LogoutAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Result.Failure("User not found.");
            user.RefreshToken = null;
            user.RefreshTokenExpireDate = null;

            await _activityLogService.AddAsync(
                ActivityAction.Logout,
                nameof(AppUser),
                user.Id,
                $"User '{user.UserName}' logged out.",
                user.Id);

            await _userManager.UpdateAsync(user);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Logged out successfully.");
        }

        public async Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(x => x.RefreshToken == dto.RefreshToken);

            if (user is null)
                return Result<LoginResponseDto>.Failure("Invalid refresh token.");
            if (!user.RefreshTokenExpireDate.HasValue ||
                user.RefreshTokenExpireDate <= DateTime.UtcNow)
            {
                return Result<LoginResponseDto>.Failure("Refresh token expired.");
            }
            _businessRules.EnsureUserIsActive(user);

            LoginResponseDto response = await BuildLoginResponseAsync(user);

            return Result<LoginResponseDto>.Success(response);
        }

        public async Task<Result> RegisterAsync(RegisterDto dto)
        {
            var user = new AppUser()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                UserName = dto.UserName,
                Email = dto.Email
            };

            IdentityResult identityResult = await _userManager.CreateAsync(user, dto.Password);

            if (!identityResult.Succeeded)
            {
                return Result.Failure(identityResult.Errors
                    .Select(x => x.Description)
                    .First());
            }

            IdentityResult roleResult =await _userManager.AddToRoleAsync(user, Roles.SalesRepresentative);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);

                return Result.Failure(roleResult.Errors.First().Description);
            }

            return Result.Success("User registered successfully.");
        }
        private async Task<LoginResponseDto> BuildLoginResponseAsync(AppUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);

            TokenUser tokenUser = new()
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Roles = roles
            };

            LoginResponseDto response = new()
            {
                AccessToken = _tokenService.CreateAccessToken(tokenUser),
                RefreshToken = _tokenService.CreateRefreshToken(),
                ExpireDate = _tokenService.GetAccessTokenExpireDate()
            };

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpireDate = _tokenService.GetRefreshTokenExpireDate();

            await _userManager.UpdateAsync(user);

            return response;
        }
    }
}
