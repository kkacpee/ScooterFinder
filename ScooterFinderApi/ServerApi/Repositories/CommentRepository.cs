using ServerApi.Persistance;
using ServerApi.Persistance.Models;
using ServerApi.Repositories.Interfaces;

namespace ServerApi.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context) { }
    }
}
