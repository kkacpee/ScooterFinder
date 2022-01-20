using Microsoft.EntityFrameworkCore;
using ServerApi.DTO.Comment;
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

        public async Task<List<Pin>> GetPinsAsync(CancellationToken cancellationToken)
        {
            return (await _pinRepository.GetAllAsync(cancellationToken)).ToList();
        }

        public async Task<PinDetailsResponse> GetPinAsync(int id, CancellationToken cancellationToken)
        {
            var pin = await _pinRepository.GetByIdAsync(id, cancellationToken, include: x => x.Include(x => x.User).Include(x => x.Comments).ThenInclude(x => x.User));
            var response = new PinDetailsResponse
            {
                Id = pin.Id,
                Coordinates = pin.Coordinates,
                CreationDate = pin.CreationDate,
                Description = pin.Description,
                DisplayName = pin.User.DisplayName,
                PinName = pin.PinName,
                PinTypeId = pin.PinTypeId,
                UserId = pin.UserId
            };

            pin.Comments.ToList().ForEach(comment =>
            {
                response.Comments.Add(new CommentResponse
                {
                    UserId = comment.UserId,
                    Content = comment.Content,
                    Date = comment.Date,
                    Id = comment.Id,
                    DisplayName = comment.User.DisplayName
                });
            });

            return response;
        }

        public async Task<IResult> AddPinAsync(AddPinRequest dto, CancellationToken cancellationToken)
        {
            var pin = new Pin
            {
                UserId = dto.UserId,
                PinName = dto.PinName,
                PinTypeId = dto.PinTypeId,
                Coordinates = dto.Coordinates,
                Description = dto.Description,
                CreationDate = DateTime.UtcNow
            };

            await _pinRepository.AddAsync(pin, cancellationToken);
            return Results.Ok();
        }

        public async Task<IResult> UpdatePinAsync(EditPinRequest dto, CancellationToken cancellationToken) 
        {
            var pin = await _pinRepository.GetByIdAsync(dto.PinId, cancellationToken);
            
            if (pin == null) return Results.NotFound();

            pin.Description = dto.Description;
            pin.PinName = dto.PinName;

            await _pinRepository.UpdateAsync(pin, cancellationToken);
            return Results.Ok(); 
        }

        public async Task<IResult> DeletePinAsync(int id, CancellationToken cancellationToken) 
        {
            await _pinRepository.DeletePermanentlyByIdAsync(id, cancellationToken);
            return Results.Ok();
        }
    }
}
