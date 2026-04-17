using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;
        [Required, MaxLength(150)]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string PasswordHash { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        [Required]
        public string Role { get; set; } = "Customer"; // Customer | Admin | Department
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
        public Cart? Cart { get; set; }
    }
}