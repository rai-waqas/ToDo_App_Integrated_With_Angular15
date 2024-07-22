using Core.Interfaces;
using Core.Models;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Core.Models.Task>> GetAllTasksAsync()
        {
            return await _unitOfWork.Tasks.GetAllAsync();
        }

        public async Task<Core.Models.Task?> GetTaskByIdAsync(int id)
        {
            return await _unitOfWork.Tasks.GetByIdAsync(id);
        }

        public async Task AddTaskAsync(Core.Models.Task task)
        {
            await _unitOfWork.Tasks.AddAsync(task);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateTaskAsync(Core.Models.Task task)
        {
            var existingTask = await _unitOfWork.Tasks.GetByIdAsync(task.Id);

            if (existingTask != null)
            {
                existingTask.Name = task.Name;
                existingTask.Description = task.Description;
                existingTask.DueDate = task.DueDate;
                existingTask.Priority = task.Priority;
                existingTask.Completed = task.Completed;
                await _unitOfWork.CompleteAsync();
            }
            else
            {
                throw new Exception("Task not Found");
            }
            
        }

        public async Task DeleteTaskAsync(int id)
        {
            await _unitOfWork.Tasks.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public Task<IEnumerable<Core.Models.Task>> GetTasksByUserId(int userId)
        {
            return _unitOfWork.Tasks.GetTasksByUserId(userId);
        }
    }
}
