using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required, MaxLength(100)]
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }

        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}