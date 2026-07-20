using FluentValidation;
using Mapster;
using Microsoft.EntityFrameworkCore;
using SalesFlow.Business.Dtos.DealDtos;
using SalesFlow.Business.Services.ActivityLogServices;
using SalesFlow.Business.Services.ExportServices;
using SalesFlow.Business.Services.RealtimeServices;
using SalesFlow.Business.Services.UserServices;
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
        private readonly IActivityLogService _activityLogService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IExcelExportService _excelExportService;
        private readonly IPdfExportService _pdfExportService;
        private readonly IRealtimeService _realtimeService;

        public DealService(IDealRepository dealRepository, IUnitOfWork unitOfWork, DealBusinessRules businessRules, IValidator<CreateDealDto> createValidator, IValidator<UpdateDealDto> updateValidator, IActivityLogService activityLogService, ICurrentUserService currentUserService, IExcelExportService excelExportService, IPdfExportService pdfExportService, IRealtimeService realtimeService)
        {
            _dealRepository = dealRepository;
            _unitOfWork = unitOfWork;
            _businessRules = businessRules;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _activityLogService = activityLogService;
            _currentUserService = currentUserService;
            _excelExportService = excelExportService;
            _pdfExportService = pdfExportService;
            _realtimeService = realtimeService;
        }

        public async Task<Result> CreateAsync(CreateDealDto dto)
        {
            await ValidateAndThrowAsync(_createValidator, dto);
            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);
            await _businessRules.EnsureAssignedUserExistsAsync(dto.AssignedUserId);
            await _businessRules.EnsureActiveDealTitleIsUniqueAsync(dto.Title,dto.CustomerId);
            var deal = dto.Adapt<Deal>();
            deal.Stage = DealStage.New;
            await _dealRepository.AddAsync(deal);
            await _activityLogService.AddAsync( ActivityAction.Create,nameof(Deal), deal.Id,$"Deal '{deal.Title}' created.", _currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            await _realtimeService.DashboardUpdatedAsync();
            return Result.Success("Deal created successfully.");
        }

        public async Task<Result> UpdateAsync(UpdateDealDto dto)
        {
            await ValidateAndThrowAsync(_updateValidator, dto);

            var deal = await _businessRules.GetDealByIdAsync(dto.Id, true);
            _businessRules.EnsureUserCanModify(deal);

            _businessRules.EnsureDealIsEditable(deal);

            await _businessRules.EnsureCustomerExistsAsync(dto.CustomerId);

            await _businessRules.EnsureAssignedUserExistsAsync(dto.AssignedUserId);

            await _businessRules.EnsureActiveDealTitleIsUniqueForUpdateAsync(
                dto.Id,
                dto.Title,
                dto.CustomerId
            );

            // Stage gerçekten değişiyorsa geçiş kontrolü yap
            if (deal.Stage != dto.Stage)
            {
                _businessRules.EnsureStageTransition(
                    deal.Stage,
                    dto.Stage
                );
            }

            dto.Adapt(deal);

            _dealRepository.Update(deal);

            await _activityLogService.AddAsync(
                ActivityAction.Update,
                nameof(Deal),
                deal.Id,
                $"Deal '{deal.Title}' updated.",
                _currentUserService.UserId
            );

            await _unitOfWork.SaveChangesAsync();

            await _realtimeService.DashboardUpdatedAsync();

            return Result.Success(
                "Deal updated successfully."
            );
        }

        public async Task<Result> DeleteAsync(int id)
        {
            var deal = await _businessRules.GetDealByIdAsync(id, true);

            _businessRules.EnsureUserCanModify(deal);

            _businessRules.EnsureDealIsDeletable(deal);

            _dealRepository.Delete(deal);
            await _activityLogService.AddAsync(ActivityAction.Delete,nameof(Deal),deal.Id,$"Deal '{deal.Title}' deleted.",_currentUserService.UserId);
            await _unitOfWork.SaveChangesAsync();
            await _realtimeService.DashboardUpdatedAsync();
            return Result.Success("Deal deleted successfully.");
        }

        public async Task<Result<PagedResult<ResultDealDto>>> GetAllAsync(PaginationRequest request)
        {
            IQueryable<Deal> query = _dealRepository.GetAll();

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }

            PagedResult<ResultDealDto> deals = await query
                .ProjectToType<ResultDealDto>()
                .ToPagedResultAsync(request);

            return Result<PagedResult<ResultDealDto>>.Success(deals);
        }

        public async Task<Result<GetByIdDealDto>> GetByIdAsync(int id)
        {
            Deal? deal = await _dealRepository
                .GetAll()
                .Include(x => x.Customer)
                .Include(x => x.AssignedUser)
                .FirstOrDefaultAsync(x => x.Id == id);


            if (deal is null)
            {
                return Result<GetByIdDealDto>.Failure(
                    "Deal not found.");
            }
            _businessRules.EnsureUserCanModify(deal);
            GetByIdDealDto dto = new()
            {
                Id = deal.Id,
                Title = deal.Title,
                Description = deal.Description,
                Amount = deal.Amount,
                ExpectedCloseDate = deal.ExpectedCloseDate,
                Stage = deal.Stage,

                CustomerId = deal.CustomerId,

                CustomerName =
                    !string.IsNullOrWhiteSpace(
                        deal.Customer.CompanyName)
                        ? deal.Customer.CompanyName
                        : $"{deal.Customer.ContactFirstName} {deal.Customer.ContactLastName}",

                AssignedUserId =
                    deal.AssignedUserId,

                AssignedUserName =
                    deal.AssignedUser is null
                        ? null
                        : $"{deal.AssignedUser.FirstName} {deal.AssignedUser.LastName}"
            };

            return Result<GetByIdDealDto>.Success(dto);
        }
        public async Task<byte[]> ExportAsync()
        {
            IQueryable<Deal> query = _dealRepository
     .GetAll()
     .Include(x => x.Customer);

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }

            List<Deal> deals = await query
                .OrderBy(x => x.Title)
                .ToListAsync();

            return _excelExportService.ExportDeals(deals);
        }
        public async Task<byte[]> ExportPdfAsync()
        {
            IQueryable<Deal> query = _dealRepository
    .GetAll()
    .Include(x => x.Customer);

            if (!_currentUserService.IsInRole("Admin") &&
                !_currentUserService.IsInRole("SalesManager"))
            {
                query = query.Where(x =>
                    x.AssignedUserId == _currentUserService.UserId);
            }
            List<Deal> deals = await _dealRepository
                .GetAll()
                .Include(x => x.Customer)
                .OrderBy(x => x.Title)
                .ToListAsync();

            return _pdfExportService.ExportDeals(deals);
        }
        private static async Task ValidateAndThrowAsync<TDto>(IValidator<TDto> validator, TDto dto)
        {
            var validationResult = await validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);
        }
    }
}