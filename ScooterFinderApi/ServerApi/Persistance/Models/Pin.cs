namespace ServerApi.Persistance.Models
{
    public partial class Pin
    {
        public int Id { get; set;}
        public string PinName { get; set;}
        public DateTime CreationDate { get; set; }
        public int PinTypeId { get; set; }
        public string Description { get; set; }
        public string Coordinates { get; set; }
        public int UserId { get; set; }

        public virtual User User { get; set; }
        public virtual PinTypes PinType { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }


    }
}
