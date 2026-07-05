using FluentValidation;
using Mapster;
using SalesFlow.Business.Dtos.TagDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
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

        public TagService(ITagRepository tagRepository,IUnitOfWork unitOfWork, TagBusinessRules businessRules, IValidator<CreateTagDto> createValidator, IValidator<UpdateTagDto> updateValidator)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result> CreateAsync(CreateTagDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _businessRules.EnsureTagNameIsUniqueAsync(dto.Name);
            var tag = dto.Adapt<Tag>();
            await _tagRepository.AddAsync(tag);
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
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Tag updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var tag = await _businessRules.GetTagByIdAsync(id, true);
            _tagRepository.Delete(tag);
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
