using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DULUXFINALPART1.Models
{
    public class Return
    {
        [Key]

        public int Id { get; set; }

        // Foreign key to Scan_Image
        public int ScanImageId { get; set; }

        [ForeignKey("ScanImageId")]
        public Scan_Image? ScanImage { get; set; }

        // DeliveryNumber Property
        public string? DeliveryNumber { get; set; }

        // Optionally pull in some fields for easy access
        public string? Shipment => ScanImage?.Shipment;
        public DateTime? CreatedAt => ScanImage?.CreatedAt;

        // Your new field
        public string? Comments { get; set; }

        public DateTime? ReturnCreatedAt { get; set; }

        // Function to set the time when the return is created
        public void SetReturnCreatedAt()
        {
            ReturnCreatedAt = DateTime.Now;
        }
    }
}