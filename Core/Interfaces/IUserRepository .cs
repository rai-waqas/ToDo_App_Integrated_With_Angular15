﻿using Core.Models;

namespace Core.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetUserByUsernameAsync(string username);
    }
}
