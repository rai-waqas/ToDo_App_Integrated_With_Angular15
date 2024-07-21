using Core.Interfaces;
using Infrastructure.DataContext;

namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DBContext _context;
        public UnitOfWork(DBContext context) 
        {
            _context = context;
            Tasks = new TaskRepository(_context);
            Users = new UserRepository(_context);
        }

        public ITaskRepository Tasks { get; private set; }
        public IUserRepository Users { get; private set; }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
