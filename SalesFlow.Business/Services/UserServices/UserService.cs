using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesFlow.Business.Dtos.UserDtos;
using SalesFlow.Core.Results;
using SalesFlow.Entity.Entities;

namespace SalesFlow.Business.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<List<ResultUserDto>>> GetAllAsync()
        {
            List<ResultUserDto> users =
                await _userManager.Users
                    .Where(x => x.IsActive)
                    .OrderBy(x => x.FirstName)
                    .ThenBy(x => x.LastName)
                    .Select(x => new ResultUserDto
                    {
                        Id = x.Id,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        UserName = x.UserName!
                    })
                    .ToListAsync();

            return Result<List<ResultUserDto>>
                .Success(users);
        }
    }
}