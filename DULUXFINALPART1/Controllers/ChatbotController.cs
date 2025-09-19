using Microsoft.AspNetCore.Mvc;
using DULUXFINALPART1.Data;
using System.Linq;

[ApiController]
[Route("api/chatbot")]
public class ChatbotController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ChatbotController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("shipmentstatus")]
    public IActionResult GetShipmentStatus(string shipmentId)
    {
        if (string.IsNullOrWhiteSpace(shipmentId))
            return BadRequest("Missing shipment ID.");

        var shipment = _context.Scan_Images
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefault(s => s.Shipment == shipmentId);

        if (shipment == null)
            return NotFound($"No shipment found for ID {shipmentId}.");

        string status = shipment.TimeTakenSeconds switch
        {
            > 120 => "Very Long",
            > 60 => "Long",
            > 0 => "Done",
            _ => "Busy"
        };

        return Ok($"Shipment {shipmentId} is currently: {status}.");
    }
}
