using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUnitOfWork
    {
        ITaskRepository Tasks { get; }
        IUserRepository Users { get; }
        ITagRepository Tags { get; }
        Task<int> CompleteAsync();
    }
}
