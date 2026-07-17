using Microsoft.EntityFrameworkCore;
using SalesFlow.Business.Dtos.NotificationDtos;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.NotificationRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;

namespace SalesFlow.Business.Services.NotificationServices
{
    public class NotificationService
        : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationBusinessRules _businessRules;
        private readonly IRealtimeService _realtimeService;
        public NotificationService(INotificationRepository repository, IUnitOfWork unitOfWork, NotificationBusinessRules businessRules, IRealtimeService realtimeService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _realtimeService = realtimeService;
        }

        public async Task AddAsync(int userId,string title,string message,NotificationType type,string? entityName = null,int? entityId = null)
        {
            Notification notification = new()
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                IsRead = false,
                EntityName = entityName,
                EntityId = entityId
            };

            await _repository.AddAsync(notification);

            await _unitOfWork.SaveChangesAsync();
            ResultNotificationDto notificationDto = new()
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                EntityName = notification.EntityName,
                EntityId = notification.EntityId,
                CreatedDate = notification.CreatedDate
            };

            await _realtimeService.SendNotificationAsync(
                userId,
                notificationDto);
        }

        public async Task<Result<PagedResult<ResultNotificationDto>>>GetAllAsync(int userId,NotificationFilterRequest request)
        {
            IQueryable<Notification> query = _repository.GetAll().Where(x => x.UserId == userId);

            if (request.Type.HasValue)
            {
                query = query.Where( x => x.Type == request.Type.Value);
            }

            if (request.IsRead.HasValue)
            {
                query = query.Where( x => x.IsRead == request.IsRead.Value);
            }

            PagedResult<ResultNotificationDto> notifications =await query.OrderByDescending(x => x.CreatedDate).Select(x => new ResultNotificationDto
                    {
                        Id = x.Id,
                        Title = x.Title,
                        Message = x.Message,
                        Type = x.Type,
                        IsRead = x.IsRead,
                        EntityName = x.EntityName,
                        EntityId = x.EntityId,
                        CreatedDate = x.CreatedDate
                    })
                    .ToPagedResultAsync(request);
            return Result<PagedResult<ResultNotificationDto>>.Success(notifications);
        }

        public async Task<Result<GetByIdNotificationDto>>GetByIdAsync(int id,int userId)
        {
            Notification notification =await _businessRules.GetByIdAsync(id, userId);

            GetByIdNotificationDto dto = new()
            {
                Id = notification.Id,
                Title = notification.Title,
                Message = notification.Message,
                Type = notification.Type,
                IsRead = notification.IsRead,
                EntityName = notification.EntityName,
                EntityId = notification.EntityId,
                CreatedDate = notification.CreatedDate
            };

            return Result<GetByIdNotificationDto>.Success(dto);
        }

        public async Task<Result<int>> GetUnreadCountAsync(int userId)
        {
            int count =await _repository.GetAll().CountAsync( x => x.UserId == userId &&!x.IsRead);

            return Result<int>.Success(count);
        }

        public async Task<Result> MarkAsReadAsync(int id,int userId)
        {
            Notification notification = await _businessRules .GetByIdAsync(id, userId);

            if (notification.IsRead)
            {
                return Result.Success("Notification is already marked as read.");
            }

            notification.IsRead = true;

            _repository.Update(notification);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success(
                "Notification marked as read.");
        }

        public async Task<Result>
            MarkAllAsReadAsync(
                int userId)
        {
            List<Notification> notifications =
                await _repository
                    .GetAll()
                    .Where(
                        x =>
                            x.UserId == userId &&
                            !x.IsRead)
                    .ToListAsync();

            if (notifications.Count == 0)
            {
                return Result.Success(
                    "There are no unread notifications.");
            }

            foreach (Notification notification in notifications)
            {
                notification.IsRead = true;

                _repository.Update(notification);
            }

            await _unitOfWork.SaveChangesAsync();

            return Result.Success(
                "All notifications marked as read.");
        }
    }
}