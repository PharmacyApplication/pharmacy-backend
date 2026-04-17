using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.DTOs.Medicine
{
    public class CreateMedicineDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool RequiresPrescription { get; set; }
        public string ImageUrl { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
    }

    public class UpdateMedicineDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool RequiresPrescription { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
        public int QuantityInStock { get; set; }   // synced to inventory
        public int ReorderLevel { get; set; }      // synced to inventory
    }

}
