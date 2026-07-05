using SalesFlow.Business.Dtos.AuthDtos;
using SalesFlow.Business.Dtos.JwtDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.JwtServices
{
    public interface ITokenService
    {
        string CreateAccessToken(TokenUser user);

        string CreateRefreshToken();

        DateTime GetAccessTokenExpireDate();

        DateTime GetRefreshTokenExpireDate();
    }
}
