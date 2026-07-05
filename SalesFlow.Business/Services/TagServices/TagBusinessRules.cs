using SalesFlow.Core.Exceptions;
using SalesFlow.DataAccess.Repositories.TagRepositories;
using SalesFlow.Entity.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SalesFlow.Business.Services.TagServices
{
    public class TagBusinessRules
    {
        private readonly ITagRepository _tagRepository;

        public TagBusinessRules(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<Tag> GetTagByIdAsync(int id, bool tracking = false)
        {
            var tag = await _tagRepository.GetByIdAsync(id, tracking);
            if (tag is null)
                throw new NotFoundException("Tag not found.");
            return tag;
        }

        public async Task EnsureTagNameIsUniqueAsync(string name)
        {
            var exists = await _tagRepository.AnyAsync(x => x.Name == name);
            if (exists)
                throw new BusinessException("A tag with the same name already exists.");
        }

        public async Task EnsureTagNameIsUniqueForUpdateAsync(int id, string name)
        {
            var exists = await _tagRepository.AnyAsync(x =>x.Id != id &&x.Name == name);
            if (exists)
                throw new BusinessException("A tag with the same name already exists.");
        }
    }
}
