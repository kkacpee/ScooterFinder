namespace ServerApi.Persistance.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int UserId { get; set; }
        public int PinId { get; set; }
        public DateTime Date { get; set; }

        public virtual User User { get; set; }
        public virtual Pin Pin { get; set; }
    }
}
