using Core.Interfaces;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
using Task = Core.Models.Task;

namespace Infrastructure.Repositories
{
    public class TaskRepository : GenericRepository<Task>, ITaskRepository
    {
        private new readonly DBContext _context;
        public TaskRepository(DBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Task>> GetTasksByUserId(int userId)
        {
            return await _context.Tasks
                                 .Where(t => t.UserId == userId)
                                 .ToListAsync();
        }
    }
}
