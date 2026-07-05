using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.AttachmentRepositories;
using SalesFlow.DataAccess.Repositories.CustomerRepositories;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.AttachmentServices
{
    public class AttachmentBusinessRules
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly ICustomerRepository _customerRepository;

        public AttachmentBusinessRules(IAttachmentRepository attachmentRepository,ICustomerRepository customerRepository)
        {
            _attachmentRepository = attachmentRepository;
            _customerRepository = customerRepository;
        }

        public async Task<Attachment> GetAttachmentByIdAsync(int id, bool tracking = false)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(id, tracking);

            if (attachment is null)
                throw new NotFoundException("Attachment not found.");

            return attachment;
        }

        public async Task EnsureCustomerExistsAsync(int customerId)
        {
            var exists = await _customerRepository.AnyAsync(x => x.Id == customerId);

            if (!exists)
                throw new BusinessException("Customer not found.");
        }
    }
}
