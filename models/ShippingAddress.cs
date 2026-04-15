    public class ShippingAddress
    {
        public required string Name { get; set; }
        public required string AddressLine1 { get; set; }
        public string? City { get; set; }
        public string? StateOrRegion { get; set; }
        public required string PostalCode { get; set; }
        public required string CountryCode { get; set; }
        public string? Phone { get; set; }
        public string? AddressType { get; set; }
    }
