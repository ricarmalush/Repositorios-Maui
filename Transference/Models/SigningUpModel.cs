namespace Transference.Models
{
    public class SigningUpModel
    {
        public int UserId { get; set; }
        public DateTime SigningDateTime { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string ClockCreateBy { get; set; }
        public string Type { get; set; }
        public int ProvinceId { get; set; }
        public string Province { get; set; }
        public int CommunityId { get; set; }
        public string Community { get; set; }
        public string Street { get; set; }
    }
}
