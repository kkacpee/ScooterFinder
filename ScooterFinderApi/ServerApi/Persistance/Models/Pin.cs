namespace ServerApi.Persistance.Models
{
    public partial class Pin
    {
        public Guid PinId { get; set;}
        public string PinName { get; set;}
        public int PinTypeId { get; set; }
        public string Description { get; set; }
        public string Coordinates { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }


    }
}
