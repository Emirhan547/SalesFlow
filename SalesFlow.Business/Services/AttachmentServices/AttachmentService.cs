using FluentValidation;
using Mapster;
using SalesFlow.Business.Dtos.AttachmentDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.FileServices;
using SalesFlow.Business.Services.UserServices;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.AttachmentRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
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
        private readonly IActivityLogService _activityLogService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileService _fileService;
        public AttachmentService(IAttachmentRepository attachmentRepository, IUnitOfWork unitOfWork, AttachmentBusinessRules businessRules, IValidator<CreateAttachmentDto> createValidator, IActivityLogService activityLogService, ICurrentUserService currentUserService, IFileService fileService)
        {
            _attachmentRepository = attachmentRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _activityLogService = activityLogService;
            _currentUserService = currentUserService;
            _fileService = fileService;
        }

        public async Task<Result> CreateAsync(CreateAttachmentDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            FileUploadResult upload =
     await _fileService.UploadAsync(dto.File);

            Attachment attachment = new()
            {
                FileName = upload.FileName,

                FilePath = upload.FilePath,

                ContentType = upload.ContentType,

                FileSize = upload.FileSize,

                CustomerId = dto.CustomerId
            };

            await _attachmentRepository.AddAsync(attachment);

            await _activityLogService.AddAsync(
                ActivityAction.Create,
                nameof(Attachment),
                attachment.Id,
                $"Attachment '{attachment.FileName}' uploaded.",
                _currentUserService.UserId);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success("Attachment uploaded successfully.");
        }

        

        public async Task<Result> DeleteAsync(int id)
        {
            var attachment = await _businessRules.GetAttachmentByIdAsync(id, true);
            await _fileService.DeleteAsync(attachment.FilePath);

            _attachmentRepository.Delete(attachment);

            await _activityLogService.AddAsync(
                ActivityAction.Delete,
                nameof(Attachment),
                attachment.Id,
                $"Attachment '{attachment.FileName}' deleted.",
                _currentUserService.UserId);

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
        public async Task<DownloadAttachmentDto> DownloadAsync(int id)
        {
            Attachment attachment =
                await _businessRules.GetAttachmentByIdAsync(id);

            byte[] fileBytes =
                await _fileService.ReadAsync(attachment.FilePath);

            return new DownloadAttachmentDto
            {
                FileBytes = fileBytes,
                FileName = attachment.FileName,
                ContentType = attachment.ContentType
            };
        }
    }
}
