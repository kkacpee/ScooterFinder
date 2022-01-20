using ServerApi.DTO.Comment;

namespace ServerApi.Services
{
    public interface ICommentService
    {
        Task<IResult> AddCommentAsync(AddCommentRequest request, CancellationToken cancellationToken);
        Task<IResult> DeleteCommentAsync(int id, CancellationToken cancellationToken);
    }
}