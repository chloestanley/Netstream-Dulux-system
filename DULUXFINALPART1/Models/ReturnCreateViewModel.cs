using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DULUXFINALPART1.Models
{
    public class ReturnCreateViewModel
    {
        [Required(ErrorMessage = "Please select a shipment first.")]
        public int? ScanImageId { get; set; }

        // List of deliveries user can choose
        public List<string>? AvailableDeliveries { get; set; } = new List<string>();

        // The deliveries the user selects (via checkboxes)
        public List<string>? SelectedDeliveries { get; set; } = new List<string>();

        public string? Comments { get; set; }
    }
}
