namespace Transference.Models
{
    public class NominatimResponse
    {
        public string PlaceId { get; set; }
        public string Licence { get; set; }
        public string OSMType { get; set; }
        public long OSMId { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string Class { get; set; }
        public string Type { get; set; }
        public int PlaceRank { get; set; }
        public double Importance { get; set; }
        public string Addresstype { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public Address Address { get; set; } // Nuevo objeto para la dirección
    }

    public class Address
    {
        public string Road { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string Province { get; set; }
        public string State { get; set; }
        public string Postcode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }


}
