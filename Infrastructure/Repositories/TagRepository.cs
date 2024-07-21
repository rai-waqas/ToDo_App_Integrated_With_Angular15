using Core.Interfaces;
using Core.Models;
using Infrastructure.DataContext;

namespace Infrastructure.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {

        public TagRepository(DBContext context) : base(context)
        {
        }
    }
}
