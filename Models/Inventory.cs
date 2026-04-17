using System.ComponentModel.DataAnnotations;

namespace PharmacyAPI.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }
        public int MedicineId { get; set; }
        public int QuantityInStock { get; set; } = 0;
        public int ReorderLevel { get; set; } = 10;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public Medicine? Medicine { get; set; }
    }
}