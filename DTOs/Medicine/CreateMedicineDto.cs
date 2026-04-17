using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.DTOs.Medicine
{
    public class CreateMedicineDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool RequiresPrescription { get; set; } = false;

        public string ImageUrl { get; set; }

        [Range(0, int.MaxValue)]
        public int QuantityInStock { get; set; } = 0;   // ← add

        [Range(0, int.MaxValue)]
        public int ReorderLevel { get; set; } = 10;     // ← add
    }

    public class UpdateMedicineDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public bool RequiresPrescription { get; set; }

        public string ImageUrl { get; set; }

        public bool IsActive { get; set; }

        
    }

}
