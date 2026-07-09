using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.ActivityRepositories;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.ActivityLogServices
{
    public class ActivityLogBusinessRules
    {
        private readonly IActivityLogRepository _repository;

        public ActivityLogBusinessRules(IActivityLogRepository repository)
        {
            _repository = repository;
        }

        public async Task<ActivityLog> GetByIdAsync(int id)
        {
            ActivityLog? activity = await _repository.GetByIdAsync(id);

            if (activity is null)
                throw new NotFoundException("Activity log not found.");

            return activity;
        }
    }
}
