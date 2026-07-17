using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesFlow.Business.Dtos.NotificationDtos;
using SalesFlow.Business.Services.NotificationServices;
using SalesFlow.Core.Extensions;

namespace SalesFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationsController(
            INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] NotificationFilterRequest request)
        {
            int userId =
                int.Parse(User.FindFirst("sub")!.Value);

            var result =
                await _notificationService.GetAllAsync(
                    userId,
                    request);

            return this.ToActionResult(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(
            int id)
        {
            int userId =
                int.Parse(User.FindFirst("sub")!.Value);

            var result =
                await _notificationService.GetByIdAsync(
                    id,
                    userId);

            return this.ToActionResult(result);
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            int userId =
                int.Parse(User.FindFirst("sub")!.Value);

            var result =
                await _notificationService.GetUnreadCountAsync(
                    userId);

            return this.ToActionResult(result);
        }

        [HttpPatch("{id:int}/read")]
        public async Task<IActionResult> MarkAsRead(
            int id)
        {
            int userId =
                int.Parse(User.FindFirst("sub")!.Value);

            var result =
                await _notificationService.MarkAsReadAsync(
                    id,
                    userId);

            return this.ToActionResult(result);
        }

        [HttpPatch("read-all")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            int userId =
                int.Parse(User.FindFirst("sub")!.Value);

            var result =
                await _notificationService.MarkAllAsReadAsync(
                    userId);

            return this.ToActionResult(result);
        }
    }
}