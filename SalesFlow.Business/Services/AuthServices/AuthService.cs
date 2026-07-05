using Microsoft.AspNetCore.Identity;
using SalesFlow.Business.Dtos.AuthDtos;
using SalesFlow.Business.Dtos.JwtDtos;
using SalesFlow.Business.Services.JwtServices;
using SalesFlow.Core.Results;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace SalesFlow.Business.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AuthBusinessRules _businessRules;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<AppUser> userManager, AuthBusinessRules businessRules, ITokenService tokenService)
        {
            _userManager = userManager;
            _businessRules = businessRules;
            _tokenService = tokenService;
        }

        public async Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto)
        {
            var user = await _businessRules.GetUserByEmailAsync(dto.Email);
            _businessRules.EnsureUserIsActive(user);
            bool checkPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!checkPassword)
                return Result<LoginResponseDto>.Failure("Invalid email or password.");

            var roles = await _userManager.GetRolesAsync(user);

            var tokenUser = new TokenUser()
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Roles = roles
            };
            var response = new LoginResponseDto()
            {
                AccessToken = _tokenService.CreateAccessToken(tokenUser),
                RefreshToken = _tokenService.CreateRefreshToken(),
                ExpireDate = _tokenService.GetAccessTokenExpireDate()
            };
            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpireDate = _tokenService.GetRefreshTokenExpireDate();
            await _userManager.UpdateAsync(user);
            return Result<LoginResponseDto>.Success(response);
        }

        public async Task<Result> LogoutAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user is null)
                return Result.Failure("User not found.");

            user.RefreshToken = null;
            user.RefreshTokenExpireDate = null;

            await _userManager.UpdateAsync(user);

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

           var roles = await _userManager.GetRolesAsync(user);
            var tokenUser = new TokenUser()
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Roles = roles
            };

            var response = new LoginResponseDto()
            {
                AccessToken = _tokenService.CreateAccessToken(tokenUser),
                RefreshToken = _tokenService.CreateRefreshToken(),
                ExpireDate = _tokenService.GetAccessTokenExpireDate()
            };

            user.RefreshToken = response.RefreshToken;
            user.RefreshTokenExpireDate = _tokenService.GetRefreshTokenExpireDate();
            await _userManager.UpdateAsync(user);

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

            await _userManager.AddToRoleAsync(user, "SalesRepresentative");

            return Result.Success("User registered successfully.");
        }
    }
}
