using SalesFlow.Business.Dtos.NotificationDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.NotificationServices
{
    public interface INotificationService
    {
        Task AddAsync( int userId,string title,string message,NotificationType type,string? entityName = null,int? entityId = null);
        Task<Result<PagedResult<ResultNotificationDto>>> GetAllAsync(int userId,NotificationFilterRequest request);
        Task<Result<GetByIdNotificationDto>> GetByIdAsync(int id,int userId);
        Task<Result<int>> GetUnreadCountAsync(int userId);
        Task<Result> MarkAsReadAsync(int id,int userId);
        Task<Result> MarkAllAsReadAsync( int userId);
    }
}
