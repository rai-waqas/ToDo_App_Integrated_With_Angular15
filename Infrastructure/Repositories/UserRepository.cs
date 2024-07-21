using Core.Interfaces;
using Core.Models;
using Infrastructure.DataContext;
using Microsoft.EntityFrameworkCore;
namespace Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private new readonly DBContext _context;
        public UserRepository(DBContext context) : base(context)
        {
            _context = context;
        }
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .SingleOrDefaultAsync(u => u.Username == username);
        }
    }
}
