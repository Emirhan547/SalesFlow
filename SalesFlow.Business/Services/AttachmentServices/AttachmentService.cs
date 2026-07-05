using FluentValidation;
using Mapster;
using SalesFlow.Business.Dtos.AttachmentDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.AttachmentRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.AttachmentServices
{
    public class AttachmentService : IAttachmentService
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AttachmentBusinessRules _businessRules;
        private readonly IValidator<CreateAttachmentDto> _createValidator;
        private readonly IValidator<UpdateAttachmentDto> _updateValidator;

        public AttachmentService(IAttachmentRepository attachmentRepository,IUnitOfWork unitOfWork,AttachmentBusinessRules businessRules,IValidator<CreateAttachmentDto> createValidator,IValidator<UpdateAttachmentDto> updateValidator)
        {
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result> CreateAsync(CreateAttachmentDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            var attachment = dto.Adapt<Attachment>();
            await _attachmentRepository.AddAsync(attachment);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Attachment created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateAttachmentDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            var attachment = await _businessRules.GetAttachmentByIdAsync(dto.Id, true);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            dto.Adapt(attachment);
            _attachmentRepository.Update(attachment);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Attachment updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var attachment = await _businessRules.GetAttachmentByIdAsync(id, true);
            _attachmentRepository.Delete(attachment);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Attachment deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultAttachmentDto>>> GetAllAsync(PaginationRequest request)
        {
            var attachments = await _attachmentRepository
                .GetAll()
                .ProjectToType<ResultAttachmentDto>()
                .ToPagedResultAsync(request);

            return Result<PagedResult<ResultAttachmentDto>>.Success(attachments);
        }

        public async Task<Result<GetByIdAttachmentDto>> GetByIdAsync(int id)
        {
            var attachment = await _businessRules.GetAttachmentByIdAsync(id);

            var dto = attachment.Adapt<GetByIdAttachmentDto>();

            return Result<GetByIdAttachmentDto>.Success(dto);
        }
    }
}
