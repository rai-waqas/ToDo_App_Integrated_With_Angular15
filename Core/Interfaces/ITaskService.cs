using Core.Models;
using System.Collections.Generic;
using Task = System.Threading.Tasks.Task;

namespace Core.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<Core.Models.Task>> GetAllTasksAsync();
        Task<Core.Models.Task?> GetTaskByIdAsync(int id);
        Task AddTaskAsync(Core.Models.Task task);
        Task UpdateTaskAsync(Core.Models.Task task);
        Task DeleteTaskAsync(int id);
    }
}
