using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.DTOs.Order
{
    public class PlaceOrderDto
    {
        [Required]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}
