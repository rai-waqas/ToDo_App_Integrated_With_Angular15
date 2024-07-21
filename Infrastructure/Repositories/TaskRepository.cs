using Core.Interfaces;
using Infrastructure.DataContext;
using Task = Core.Models.Task;

namespace Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<Task>, ITaskRepository
    {

        public TaskRepository(DBContext context) : base(context)
        {
        }
    }
}
