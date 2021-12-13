using ServerApi.Enums;

namespace ServerApi.DTO.Pin
{
    public class AddPinRequest
    {
        public string PinName { get; set; }
        public PinTypes PinType { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string Coordinates { get; set; }
    }
}
