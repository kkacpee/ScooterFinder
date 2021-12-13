using ServerApi.DTO.Pin;
using ServerApi.Persistance.Models;
using ServerApi.Repositories;

namespace ServerApi.Services
{
    public class PinService : IPinService
    {
        private readonly IPinRepository _pinRepository;

        public PinService(IPinRepository pinRepository)
        {
            _pinRepository = pinRepository;
        }

        public async Task<IResult> AddPinAsync(AddPinRequest dto, CancellationToken cancellationToken)
        {
            var pin = new Pin
            {
                UserId = dto.UserId,
                PinName = dto.PinName,
                PinTypeId = (int)dto.PinType,
                Coordinates = dto.Coordinates,
                Description = dto.Description
            };

            await _pinRepository.AddAsync(pin, cancellationToken);
            return Results.Ok();
        }

        public async Task<IResult> UpdatePin() { return Results.Ok(); }

        public async Task<IResult> DeletePinAsync(int id, CancellationToken cancellationToken) 
        {
            await _pinRepository.DeleteByIdAsync(id, cancellationToken);
            return Results.Ok();
        }
    }
}
