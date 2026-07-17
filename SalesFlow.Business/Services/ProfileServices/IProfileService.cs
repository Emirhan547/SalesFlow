using SalesFlow.Business.Dtos.ProfileDtos;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.ProfileServices
{
    public interface IProfileService
    {
        Task<Result<GetProfileDto>> GetProfileAsync(int userId);
        Task<Result> UpdateProfileAsync(int userId,UpdateProfileDto dto);

        Task<Result> ChangePasswordAsync(
            int userId,
            ChangePasswordDto dto);
    }
}
