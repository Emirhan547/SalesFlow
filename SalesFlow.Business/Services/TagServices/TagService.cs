using FluentValidation;
using Mapster;
using SalesFlow.Business.Dtos.TagDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.TagServices
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly TagBusinessRules _businessRules;
        private readonly IValidator<CreateTagDto> _createValidator;
        private readonly IValidator<UpdateTagDto> _updateValidator;
        private readonly IActivityLogService _activityLogService;
        private readonly ICurrentUserService _currentUserService;
        public TagService(ITagRepository tagRepository, IUnitOfWork unitOfWork, TagBusinessRules businessRules, IValidator<CreateTagDto> createValidator, IValidator<UpdateTagDto> updateValidator, IActivityLogService activityLogService, ICurrentUserService currentUserService)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _activityLogService = activityLogService;
            _currentUserService = currentUserService;
        }

        public async Task<Result> CreateAsync(CreateTagDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _businessRules.EnsureTagNameIsUniqueAsync(dto.Name);
            var tag = dto.Adapt<Tag>();
            await _tagRepository.AddAsync(tag);
            await _activityLogService.AddAsync(ActivityAction.Create,nameof(Tag),tag.Id,$"Tag '{tag.Name}' created.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Tag created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateTagDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            var tag = await _businessRules.GetTagByIdAsync(dto.Id, true);
            await _businessRules.EnsureTagNameIsUniqueForUpdateAsync(dto.Id, dto.Name);
            dto.Adapt(tag);
            _tagRepository.Update(tag);
            await _activityLogService.AddAsync(ActivityAction.Update, nameof(Tag), tag.Id, $"Tag '{tag.Name}' updated.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Tag updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var tag = await _businessRules.GetTagByIdAsync(id, true);
            _tagRepository.Delete(tag);
            await _activityLogService.AddAsync(ActivityAction.Delete, nameof(Tag), tag.Id, $"Tag '{tag.Name}' deleted.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Tag deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultTagDto>>> GetAllAsync(PaginationRequest request)
        {
            var tags = await _tagRepository.GetAll().ProjectToType<ResultTagDto>().ToPagedResultAsync(request);
            return Result<PagedResult<ResultTagDto>>.Success(tags);
        }

        public async Task<Result<GetByIdTagDto>> GetByIdAsync(int id)
        {
            var tag = await _businessRules.GetTagByIdAsync(id);
            var dto = tag.Adapt<GetByIdTagDto>();
            return Result<GetByIdTagDto>.Success(dto);
        }
    }
}
