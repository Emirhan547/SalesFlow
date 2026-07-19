using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SalesFlow.Business.Services.UserServices
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserId
        {
            get
            {
                string? id = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return int.TryParse(id, out int userId)? userId: null;
            }
        }

        public string? UserName => _httpContextAccessor.HttpContext? .User.Identity ? .Name;
        public IReadOnlyList<string> Roles =>
    _httpContextAccessor.HttpContext?
        .User
        .FindAll(ClaimTypes.Role)
        .Select(x => x.Value)
        .ToList()
    ?? [];
        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?
                .User
                .IsInRole(role)
                ?? false;
        }
    }
}
