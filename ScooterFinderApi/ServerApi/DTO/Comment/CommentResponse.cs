namespace ServerApi.DTO.Comment
{
    public class CommentResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public DateTime Date { get; set; }
    }
}
