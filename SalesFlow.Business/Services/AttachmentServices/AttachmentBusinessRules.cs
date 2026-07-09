using SalesFlow.Business.Services.CustomerServices;
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
        private readonly CustomerBusinessRules _customerBusinessRules;
        public AttachmentBusinessRules(IAttachmentRepository attachmentRepository, ICustomerRepository customerRepository, CustomerBusinessRules customerBusinessRules)
        {
            _attachmentRepository = attachmentRepository;
            _customerRepository = customerRepository;
            _customerBusinessRules = customerBusinessRules;
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
            await _customerBusinessRules.EnsureCustomerExistsAsync(customerId);
        }
    }
}
