namespace ServerApi.DTO.Comment
{
    public class AddCommentRequest
    {
        public string Content { get; set; }
        public int UserId { get; set; }
        public int PinId { get; set; }
    }
}
