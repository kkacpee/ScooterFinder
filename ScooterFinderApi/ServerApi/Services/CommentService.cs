using ServerApi.DTO.Comment;
using ServerApi.Persistance.Models;
using ServerApi.Repositories;

namespace ServerApi.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<IResult> AddCommentAsync(AddCommentRequest request, CancellationToken cancellationToken)
        {
            var comment = new Comment
            {
                PinId = request.PinId,
                UserId = request.UserId,
                Content = request.Content,
                Date = DateTime.UtcNow
            };

            await _commentRepository.AddAsync(comment, cancellationToken);
            return Results.Ok();
        }

        public async Task<IResult> DeleteCommentAsync(int id, CancellationToken cancellationToken)
        {
            await _commentRepository.DeletePermanentlyByIdAsync(id, cancellationToken);
            return Results.Ok();
        }
    }
}
