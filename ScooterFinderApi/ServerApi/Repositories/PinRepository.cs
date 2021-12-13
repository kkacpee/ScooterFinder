using ServerApi.Persistance;
using ServerApi.Persistance.Models;

namespace ServerApi.Repositories
{
    public class PinRepository : GenericRepository<Pin>, IPinRepository
    {
        public PinRepository(AppDbContext context) : base(context) { }
    }
}
