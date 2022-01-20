using ServerApi.DTO.Comment;

namespace ServerApi.DTO.Pin
{
    public class PinDetailsResponse
    {
        public int Id { get; set; }
        public string PinName { get; set; }
        public DateTime CreationDate { get; set; }
        public int PinTypeId { get; set; }
        public string Description { get; set; }
        public string Coordinates { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }

        public ICollection<CommentResponse> Comments { get; set; } = new List<CommentResponse>();
    }
}
