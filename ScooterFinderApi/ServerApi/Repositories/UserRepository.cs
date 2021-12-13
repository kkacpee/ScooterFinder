using ServerApi.Persistance;
using ServerApi.Persistance.Models;

namespace ServerApi.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
    }
}
