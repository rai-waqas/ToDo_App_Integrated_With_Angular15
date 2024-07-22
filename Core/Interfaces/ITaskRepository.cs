
using Task = Core.Models.Task;

namespace Core.Interfaces
{
    public interface ITaskRepository : IGenericRepository<Task>
    {
        Task<IEnumerable<Task>> GetTasksByUserId(int userId);
    }
}
