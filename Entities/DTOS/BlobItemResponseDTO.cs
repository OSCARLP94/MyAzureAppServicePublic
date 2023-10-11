using Azure.Storage.Blobs.Models;

namespace MyAzureAppService.Entities.DTOS
{
	public class BlobItemResponseDTO
	{
		public BlobItemResponseDTO(BlobItem item)
        {
            this.Name = item.Name;
			this.Modified = item.Properties.LastModified.GetValueOrDefault().ToString();
            this.Size = item.Properties.ContentLength.GetValueOrDefault();
        }

		public string Name { get; set; }
		public string Modified { get; set; }
		public long Size { get; set; }
	}
}

