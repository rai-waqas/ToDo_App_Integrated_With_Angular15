
using Task = Core.Models.Task;

namespace Core.Interfaces
{
    public interface ITaskRepository : IGenericRepository<Task>
    {
        public Task<IEnumerable<Task>> GetTasksByUserId(int userId);
    }
}
