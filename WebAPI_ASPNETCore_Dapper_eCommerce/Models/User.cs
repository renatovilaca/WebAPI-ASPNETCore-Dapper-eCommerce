using Dapper.Contrib.Extensions;

namespace WebAPI_ASPNETCore_Dapper_eCommerce.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string? Name { get; set; }   
        public string? Email { get; set; }  
        public string? Gender { get; set; }
        public string? RG { get; set; }
        public string? CPF { get; set; }
        public string? Mother { get; set; }
        public string? Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        [Write(false)]
        public Contact? Contact { get; set; }
        [Write(false)]
        public ICollection<DeliveryAddress>? DeliveryAddresses { get; set;}
        [Write(false)]
        public ICollection<Department>? Departments { get; set; }
    }
}
