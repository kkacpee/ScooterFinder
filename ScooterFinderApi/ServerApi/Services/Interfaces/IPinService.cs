using ServerApi.DTO.Pin;

namespace ServerApi.Services
{
    public interface IPinService
    {
        Task<IResult> AddPinAsync(AddPinRequest dto, CancellationToken cancellationToken);
        Task<IResult> DeletePinAsync(int id, CancellationToken cancellationToken);
    }
}