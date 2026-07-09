using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.UserServices
{
    public interface ICurrentUserService
    {
        int? UserId { get; }

        string? UserName { get; }
    }
}
