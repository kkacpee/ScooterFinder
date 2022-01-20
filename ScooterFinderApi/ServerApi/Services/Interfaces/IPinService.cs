using ServerApi.DTO.Pin;
using ServerApi.Persistance.Models;

namespace ServerApi.Services
{
    public interface IPinService
    {
        Task<IResult> AddPinAsync(AddPinRequest dto, CancellationToken cancellationToken);
        Task<IResult> DeletePinAsync(int id, CancellationToken cancellationToken);
        Task<PinDetailsResponse> GetPinAsync(int id, CancellationToken cancellationToken);
        Task<List<Pin>> GetPinsAsync(CancellationToken cancellationToken);
        Task<IResult> UpdatePinAsync(EditPinRequest dto, CancellationToken cancellationToken);
    }
}