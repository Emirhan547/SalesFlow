using SalesFlow.Business.Dtos.ActivityLogDtos;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.ActivityRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.ActivityLogServices
{
    public class ActivityLogService : IActivityLogService
    {
        private readonly IActivityLogRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ActivityLogBusinessRules _businessRules;
        private readonly IRealtimeService _realtimeService;
        public ActivityLogService(IActivityLogRepository repository, IUnitOfWork unitOfWork, ActivityLogBusinessRules businessRules, IRealtimeService realtimeService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _realtimeService = realtimeService;
        }
        public async Task AddAsync(ActivityAction action,string entityName,int entityId,string description,int? userId)
        {
            ActivityLog log = new()
            {
                Action = action,
                EntityName = entityName,
                EntityId = entityId,
                Description = description,
                UserId = userId
            };

            await _repository.AddAsync(log);

            await _unitOfWork.SaveChangesAsync();
            await _realtimeService.ActivityLogCreatedAsync();
        }
        public async Task<Result<PagedResult<ResultActivityLogDto>>> GetAllAsync(ActivityLogFilterRequest request)
        {
            IQueryable<ActivityLog> query = _repository.GetAll();

            if (request.Action.HasValue)
            {
                query = query.Where(x => x.Action == request.Action);
            }

            if (!string.IsNullOrWhiteSpace(request.EntityName))
            {
                query = query.Where(x => x.EntityName == request.EntityName);
            }

            if (request.UserId.HasValue)
            {
                query = query.Where(x => x.UserId == request.UserId);
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(x => x.CreatedDate <= request.EndDate.Value);
            }

            var activities = await query
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new ResultActivityLogDto
                {
                    Id = x.Id,
                    Action = x.Action,
                    EntityName = x.EntityName,
                    EntityId = x.EntityId,
                    Description = x.Description,
                    UserId = x.UserId,
                    UserName = x.User == null
                        ? null
                        : $"{x.User.FirstName} {x.User.LastName}",
                    CreatedDate = x.CreatedDate
                })
                .ToPagedResultAsync(request);

            return Result<PagedResult<ResultActivityLogDto>>
                .Success(activities);
        }
        public async Task<Result<GetByIdActivityLogDto>> GetByIdAsync(int id)
        {
            ActivityLog activity =
                await _businessRules.GetByIdAsync(id);

            GetByIdActivityLogDto dto = new()
            {
                Id = activity.Id,
                Action = activity.Action,
                EntityName = activity.EntityName,
                EntityId = activity.EntityId,
                Description = activity.Description,
                UserId = activity.UserId,
                UserName = activity.User is null
                    ? null
                    : $"{activity.User.FirstName} {activity.User.LastName}",
                CreatedDate = activity.CreatedDate
            };

            return Result<GetByIdActivityLogDto>.Success(dto);
        }
    }
}
