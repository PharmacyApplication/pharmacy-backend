namespace PharmacyAPI.DTOs.Order
{
        public class CartItemDto
        {
            public int CartItemId { get; set; }
            public int MedicineId { get; set; }
            public string MedicineName { get; set; } = string.Empty;
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public int? PrescriptionId { get; set; }
            public decimal Subtotal => Price * Quantity;
        }
    }