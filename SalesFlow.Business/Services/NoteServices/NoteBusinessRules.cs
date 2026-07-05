using Microsoft.AspNetCore.Identity;
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
        private readonly ICustomerRepository _customerRepository;
        private readonly UserManager<AppUser> _userManager;

        public NoteBusinessRules( INoteRepository noteRepository,ICustomerRepository customerRepository, UserManager<AppUser> userManager)
        {
            _noteRepository = noteRepository;
            _customerRepository = customerRepository;
            _userManager = userManager;
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
            var exists = await _customerRepository.AnyAsync(x => x.Id == customerId);

            if (!exists)
                throw new BusinessException("Customer not found.");
        }

        public async Task EnsureCreatedByExistsAsync(int? userId)
        {
            if (!userId.HasValue)
                return;
            var user = await _userManager.FindByIdAsync(userId.Value.ToString());
            if (user is null)
                throw new BusinessException("User not found.");
        }
    }
}
