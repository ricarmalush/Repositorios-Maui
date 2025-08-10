namespace Transference.Dtos
{
    public class LoginDto
    {
        public int? UserId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
