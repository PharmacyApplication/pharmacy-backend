namespace PharmacyAPI.DTOs.Medicine
{
    public class InventoryDto
    {
        public int InventoryId { get; set; }
        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsLowStock => QuantityInStock <= ReorderLevel;
    }

    public class UpdateInventoryDto
    {
        public int QuantityInStock { get; set; }
        public int ReorderLevel { get; set; }
    }

}
