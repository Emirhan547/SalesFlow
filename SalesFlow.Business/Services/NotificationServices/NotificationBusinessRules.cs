using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.NotificationRepositories;
using SalesFlow.Entity.Entities;

namespace SalesFlow.Business.Services.NotificationServices
{
    public class NotificationBusinessRules
    {
        private readonly INotificationRepository _repository;

        public NotificationBusinessRules(
            INotificationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Notification> GetByIdAsync( int id,int userId)
        {
            Notification? notification =
                await _repository.GetByIdAsync(id);

            if (notification is null ||
                notification.UserId != userId)
            {
                throw new NotFoundException(
                    "Notification not found.");
            }

            return notification;
        }
    }
}