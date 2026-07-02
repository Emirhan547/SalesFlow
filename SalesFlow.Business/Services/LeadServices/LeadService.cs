using FluentValidation;
using Mapster;
using SalesFlow.Business.Dtos.LeadDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.LeadRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.LeadServices
{
    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly LeadBusinessRules _leadBusinessRules;
        private readonly IValidator<CreateLeadDto> _createValidator;
        private readonly IValidator<UpdateLeadDto> _updateValidator;

        public LeadService( ILeadRepository leadRepository, IUnitOfWork unitOfWork, LeadBusinessRules leadBusinessRules, IValidator<CreateLeadDto> createValidator, IValidator<UpdateLeadDto> updateValidator)
        {
            _leadRepository = leadRepository;
            _unitOfWork = unitOfWork;
            _leadBusinessRules = leadBusinessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result> CreateAsync(CreateLeadDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _leadBusinessRules.EnsureEmailIsUniqueAsync(dto.Email);
            var lead = dto.Adapt<Lead>();
            lead.Status = LeadStatus.New;
            await _leadRepository.AddAsync(lead);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Lead created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateLeadDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            await _leadBusinessRules.EnsureEmailIsUniqueForUpdateAsync(dto.Id, dto.Email);
            var lead = await _leadBusinessRules.GetLeadByIdAsync(dto.Id, true);
            dto.Adapt(lead);
            _leadRepository.Update(lead);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Lead updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var lead = await _leadBusinessRules.GetLeadByIdAsync(id, true);
            _leadRepository.Delete(lead);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Lead deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultLeadDto>>> GetAllAsync(PaginationRequest request)
        {
            var leads = await _leadRepository.GetAll().ProjectToType<ResultLeadDto>() .ToPagedResultAsync(request);
            return Result<PagedResult<ResultLeadDto>>.Success(leads);
        }

        public async Task<Result<GetByIdLeadDto>> GetByIdAsync(int id)
        {
            var lead = await _leadBusinessRules.GetLeadByIdAsync(id);
            var dto = lead.Adapt<GetByIdLeadDto>();
            return Result<GetByIdLeadDto>.Success(dto);
        }
    }
}
