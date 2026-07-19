using Microsoft.AspNetCore.Identity;
using SalesFlow.Business.Services.AuthServices;
using SalesFlow.Business.Services.CustomerServices;
using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.DataAccess.Repositories.NoteRepositories;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.NoteServices
{
    public class NoteBusinessRules
    {
        private readonly INoteRepository _noteRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly CustomerBusinessRules _customerBusinessRules;
        private readonly AuthBusinessRules _authorizationBusinessRules;


        public NoteBusinessRules(INoteRepository noteRepository, UserManager<AppUser> userManager, CustomerBusinessRules customerBusinessRules, AuthBusinessRules authorizationBusinessRules)
        {
            _noteRepository = noteRepository;

            _userManager = userManager;
            _customerBusinessRules = customerBusinessRules;
            _authorizationBusinessRules = authorizationBusinessRules;
        }

        public async Task<Note> GetNoteByIdAsync(int id, bool tracking = false)
        {
            var note = await _noteRepository.GetByIdAsync(id, tracking);
            if (note is null)
                throw new NotFoundException("Note not found.");

            return note;
        }

        public async Task EnsureCustomerExistsAsync(int customerId)
        {
            await _customerBusinessRules.EnsureCustomerExistsAsync(customerId);
        }

        public async Task EnsureCreatedByExistsAsync(int? userId)
        {
            if (!userId.HasValue)
                return;
            var user = await _userManager.FindByIdAsync(userId.Value.ToString());
            if (user is null)
                throw new BusinessException("User not found.");
        }
        public void EnsureUserCanModify(Note note)
        {
            _authorizationBusinessRules
                .EnsureCurrentUserCanAccess(note.CreatedById);
        }
    }
}
