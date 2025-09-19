using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DULUXFINALPART1.Models
{
    public class GuardsPage
    {
        [Key]
        public int GuardsId { get; set; }

        // Guard Details
        [Required(ErrorMessage = "Guard Name is required.")]
        [StringLength(100)]
        public string? Guard_Name { get; set; }

        [Required(ErrorMessage = "Guard Security Card No is required.")]
        [StringLength(50)]
        public string? Guard_SecurityCard_No { get; set; }

        // Driver Details
        [Required(ErrorMessage = "Driver Name is required.")]
        [StringLength(100)]
        public string? Driver_Name { get; set; }

        [Required(ErrorMessage = "Driver Security Card No is required.")]
        [StringLength(50)]
        public string? Driver_SecurityCard_No { get; set; }

        // Shipment Info
        [Required(ErrorMessage = "Shipment selection is required.")]
        public int ScanImageId { get; set; }

        [ForeignKey("ScanImageId")]
        public Scan_Image? ScanImage { get; set; }

        public string? Shipment => ScanImage?.Shipment;

        // Confirmation flags
        public bool IsConfirmed { get; set; }
        public bool IsRejected { get; set; }

        // Date and Time of entry
        [Required(ErrorMessage = "Date and Time is required.")]
        public DateTime DateTime { get; set; } = DateTime.Now;

        // Image URLs (paths or filenames after upload)
        [Display(Name = "Upload Driver's License")]
        public string? ImageUrl1 { get; set; }

        [Display(Name = "Upload Vehicle License")]
        public string? ImageUrl2 { get; set; }

        [Display(Name = "Upload Trailer License")]
        public string? ImageUrl3 { get; set; }

        [Display(Name = "Upload Shipment Image")]
        public string? ImageUrl4 { get; set; }

        // Extracted text from scanned images (optional)
        public string? ExtractedText { get; set; }

        // Signature as Base64 string from canvas
        [Display(Name = "Signature")]
        public string? Signature { get; set; }

        // Acceptance status enum (Accept or Decline)
        [Required(ErrorMessage = "Acceptance status is required.")]
        [Display(Name = "Acceptance")]
        [Column(TypeName = "nvarchar(max)")]
        public AcceptanceStatus Acceptance { get; set; }

        // Control Room forwarding
        [Display(Name = "Send to Control Room")]
        public bool SendToControlRoom { get; set; }

        // Vehicle License scanned data (optional, add if you want to save parsed fields)
        [StringLength(50)] public string? VehicleLicenseDiscNumber { get; set; }
        [StringLength(50)] public string? VehicleRegNumber { get; set; }
        [StringLength(50)] public string? VehiclePlateNumber { get; set; }
        [StringLength(50)] public string? VehicleType { get; set; }
        [StringLength(50)] public string? VehicleMake { get; set; }
        [StringLength(50)] public string? VehicleModel { get; set; }
        [StringLength(50)] public string? VehicleColor { get; set; }
        [StringLength(50)] public string? VehicleVin { get; set; }
        [StringLength(50)] public string? VehicleEngineNumber { get; set; }
        [StringLength(50)] public string? VehicleExpiryDate { get; set; }

        // Trailer License scanned data (optional)
        [StringLength(50)] public string? TrailerDiscNumber { get; set; }
        [StringLength(50)] public string? TrailerRegNumber { get; set; }
        [StringLength(50)] public string? TrailerPlateNumber { get; set; }
        [StringLength(50)] public string? TrailerType { get; set; }
        [StringLength(50)] public string? TrailerMake { get; set; }
        [StringLength(50)] public string? TrailerModel { get; set; }
        [StringLength(50)] public string? TrailerColor { get; set; }
        [StringLength(50)] public string? TrailerVin { get; set; }
        [StringLength(50)] public string? TrailerEngineNumber { get; set; }
        [StringLength(50)] public string? TrailerExpiryDate { get; set; }
    }

    public enum AcceptanceStatus
    {
        [Display(Name = "Yes, I Accept")]
        Accept,

        [Display(Name = "No, I Decline")]
        Decline
    }
}
