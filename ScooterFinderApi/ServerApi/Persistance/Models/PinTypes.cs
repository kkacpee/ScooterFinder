namespace ServerApi.Persistance.Models
{
    public partial class PinTypes
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Pin> Pins { get; set; }
    }
}
