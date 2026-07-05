using SalesFlow.Business.Dtos.AuthDtos;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.AuthServices
{
    public interface IAuthService
    {
        Task<Result> RegisterAsync(RegisterDto dto);
        Task<Result<LoginResponseDto>> LoginAsync(LoginDto dto);
        Task<Result<LoginResponseDto>> RefreshTokenAsync(RefreshTokenDto dto);
        Task<Result> LogoutAsync(int userId);
    }
}
