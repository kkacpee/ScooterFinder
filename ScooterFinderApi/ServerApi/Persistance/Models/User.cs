namespace ServerApi.Persistance.Models
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Pin> Pins { get; set; }
    }
}
