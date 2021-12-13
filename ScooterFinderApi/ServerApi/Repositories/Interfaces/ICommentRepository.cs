using ServerApi.Interfaces.Repositories;
using ServerApi.Persistance.Models;

namespace ServerApi.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
    }
}