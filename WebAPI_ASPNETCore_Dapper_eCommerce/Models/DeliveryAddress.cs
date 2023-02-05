namespace WebAPI_ASPNETCore_Dapper_eCommerce.Models
{
    public class DeliveryAddress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? AddressTitle { get; set; }
        public string? City { get; set; }
        public string? District { get; set; }
        public string? Zipcode { get; set; }
        public string? State { get; set; }        
        public string? Address { get; set; }
        public string? Number { get; set; }
        public string? Address2 { get; set; }
        public User? User { get; set; }

    }
}
