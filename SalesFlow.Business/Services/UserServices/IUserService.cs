using SalesFlow.Business.Dtos.UserDtos;
using SalesFlow.Core.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.UserServices
{
    public interface IUserService
    {
        Task<Result<List<ResultUserDto>>> GetAllAsync();
    }
}
