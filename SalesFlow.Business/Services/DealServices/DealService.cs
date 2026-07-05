using FluentValidation;
using Mapster;
using SalesFlow.Business.Dtos.DealDtos;
using SalesFlow.Core.Paginations;
using SalesFlow.Core.Results;
using SalesFlow.DataAccess.Repositories.DealRepositories;
using SalesFlow.DataAccess.Uows;
using SalesFlow.Entity.Entities;
using SalesFlow.Entity.Enums;

namespace SalesFlow.Business.Services.DealServices
{
    public class DealService : IDealService
    {
        private readonly IDealRepository _dealRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly DealBusinessRules _businessRules;
        private readonly IValidator<CreateDealDto> _createValidator;
        private readonly IValidator<UpdateDealDto> _updateValidator;

        public DealService(IDealRepository dealRepository,IUnitOfWork unitOfWork,DealBusinessRules businessRules,IValidator<CreateDealDto> createValidator,IValidator<UpdateDealDto> updateValidator)
        {
            _dealRepository = dealRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<Result> CreateAsync(CreateDealDto dto)
        {
            await _createValidator.ValidateAndThrowAsync(dto);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            await _businessRules.EnsureAssignedUserExistsAsync(dto.AssignedUserId);
            await _businessRules.EnsureActiveDealTitleIsUniqueAsync(dto.Title,dto.CustomerId);
            var deal = dto.Adapt<Deal>();
            deal.Stage = DealStage.New;
            await _dealRepository.AddAsync(deal);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Deal created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateDealDto dto)
        {
            await _updateValidator.ValidateAndThrowAsync(dto);
            var deal = await _businessRules.GetDealByIdAsync(dto.Id, true);
            _businessRules.EnsureDealIsEditable(deal);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            await _businessRules.EnsureAssignedUserExistsAsync(dto.AssignedUserId);
            await _businessRules.EnsureActiveDealTitleIsUniqueForUpdateAsync( dto.Id,dto.Title, dto.CustomerId);
            _businessRules.EnsureStageChanged(deal.Stage,dto.Stage);
            _businessRules.EnsureStageTransition(deal.Stage,dto.Stage);
            dto.Adapt(deal);
            _dealRepository.Update(deal);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Deal updated successfully.");
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var deal = await _businessRules.GetDealByIdAsync(id, true);
            _businessRules.EnsureDealIsDeletable(deal);
            _dealRepository.Delete(deal);
            await _unitOfWork.SaveChangesAsync();
            return Result.Success("Deal deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultDealDto>>> GetAllAsync(PaginationRequest request)
        {
            var deals = await _dealRepository.GetAll().ProjectToType<ResultDealDto>() .ToPagedResultAsync(request);
            return Result<PagedResult<ResultDealDto>>.Success(deals);
        }

        public async Task<Result<GetByIdDealDto>> GetByIdAsync(int id)
        {
            var deal = await _businessRules.GetDealByIdAsync(id);
            var dto = deal.Adapt<GetByIdDealDto>();
            return Result<GetByIdDealDto>.Success(dto);
        }
    }
}