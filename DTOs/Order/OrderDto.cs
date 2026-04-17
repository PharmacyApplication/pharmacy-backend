using System;
using System.Collections.Generic;

namespace PharmacyAPI.DTOs.Order
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty; // Ordered / Shipped / Delivered / Cancelled
        public string ShippingAddress { get; set; } = string.Empty;
        public DateTime PlacedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int OrderItemId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public int? PrescriptionId { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;
    }
}