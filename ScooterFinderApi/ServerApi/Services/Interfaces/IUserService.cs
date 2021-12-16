using ServerApi.DTO;
using ServerApi.Persistance.Models;

namespace ServerApi.Services
{
    public interface IUserService
    {
        Task<User> GetUser(int id, CancellationToken cancellationToken);
        Task<IResult> Login(LoginRequest dto, IConfiguration configuration, CancellationToken cancellationToken);
        Task<IResult> Register(RegisterRequest dto, CancellationToken cancellationToken);
    }
}
