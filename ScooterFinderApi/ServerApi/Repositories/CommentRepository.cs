using ServerApi.Persistance;
using ServerApi.Persistance.Models;

namespace ServerApi.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        public CommentRepository(AppDbContext context) : base(context) { }
    }
}
