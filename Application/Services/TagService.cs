using Core.Interfaces;
using Core.Models;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace Application.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TagService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Tag>> GetAllTagsAsync()
        {
            return await _unitOfWork.Tags.GetAllAsync();
        }

        public async Task<Tag?> GetTagByIdAsync(int id)
        {
            return await _unitOfWork.Tags.GetByIdAsync(id);
        }

        public async Task AddTagAsync(Tag tag)
        {
            await _unitOfWork.Tags.AddAsync(tag);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateTagAsync(Tag tag)
        {
            var existingTag = await _unitOfWork.Tags.GetByIdAsync(tag.Id);

            if (existingTag == null)
            {
                throw new Exception("Tag not found.");
            }
            existingTag.Name = tag.Name;
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteTagAsync(int id)
        {
            await _unitOfWork.Tags.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
