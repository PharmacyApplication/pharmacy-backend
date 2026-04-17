using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        public int CartId { get; set; }
        public int MedicineId { get; set; }
        public int Quantity { get; set; } = 1;
        public int? PrescriptionId { get; set; }

        public Cart? Cart { get; set; }
        public Medicine? Medicine { get; set; }
    }
}