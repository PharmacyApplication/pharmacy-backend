using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.Models
{
    public class EmailLog
    {
        [Key]
        public int EmailLogId { get; set; }
        public int UserId { get; set; }
        public int OrderId { get; set; }
        public string EmailType { get; set; } = string.Empty; // OrderConfirmation | Shipped | Delivered
        public DateTime SentAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Sent"; // Sent | Failed
        public string? ErrorMessage { get; set; }

        public Order? Order { get; set; }
    }
}