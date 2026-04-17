namespace PharmacyAPI.DTOs.Order
{
    public class CartDto
    {
        public int CartId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<CartItemDto> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
    }
}
