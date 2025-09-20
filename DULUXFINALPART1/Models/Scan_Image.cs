using Azure.Storage.Blobs;
using static System.Net.Mime.MediaTypeNames;

namespace DULUXFINALPART1.Models
{
    public class Scan_Image
    {
        public int Id { get; set; }
        public string? ContentType { get; set; }
        public byte[]? ImageData { get; set; }
        public string? Extracted_Text { get; set; }
        public string? Shipment { get; set; }
        public bool IsSelected { get; set; }
        public string? ImageUri { get; set; }

        // ✅ Add this field for date/time
        public DateTime CreatedAt { get; set; }

        public int? TimeTakenSeconds { get; set; }

        public string? DeliveryNo1 { get; set; }
        public string? DeliveryNo2 { get; set; }
        public string? DeliveryNo3 { get; set; }
        public string? DeliveryNo4 { get; set; }
        public string? DeliveryNo5 { get; set; }
        public string? DeliveryNo6 { get; set; }
        public string? DeliveryNo7 { get; set; }
        public string? DeliveryNo8 { get; set; }
        public string? DeliveryNo9 { get; set; }
        public string? DeliveryNo10 { get; set; }
        public string? DeliveryNo11 { get; set; }
        public string? DeliveryNo12 { get; set; }
        public string? DeliveryNo13 { get; set; }
        public string? DeliveryNo14 { get; set; }
        public string? DeliveryNo15 { get; set; }
        public string? DeliveryNo16 { get; set; }
        public string? DeliveryNo17 { get; set; }
        public string? DeliveryNo18 { get; set; }
        public string? DeliveryNo19 { get; set; }
        public string? DeliveryNo20 { get; set; }
        public string? DeliveryNo21 { get; set; }
        public string? DeliveryNo22 { get; set; }
        public string? DeliveryNo23 { get; set; }
        public string? DeliveryNo24 { get; set; }
        public string? DeliveryNo25 { get; set; }


        public string? TotalPC { get; set; }
        public string? TotalVolume { get; set; }

        public string? Carrier { get; set; }





        public string GetImageBase64()
        {
            if (ImageData == null || ImageData.Length == 0)
                return string.Empty;

            return $"data:{ContentType};base64,{Convert.ToBase64String(ImageData)}";
        }

        public async Task<string> UploadToBlobAsync(string connectionString, string containerName)
        {
            if (ImageData == null || ImageData.Length == 0)
                return string.Empty;

            var blobServiceClient = new BlobServiceClient(connectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            var blobClient = blobContainerClient.GetBlobClient($"{Id}.png");

            using (var stream = new MemoryStream(ImageData))
            {
                await blobClient.UploadAsync(stream, true);
            }

            return blobClient.Uri.ToString();
        }
    }
}
