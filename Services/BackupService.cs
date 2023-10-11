using System;
using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MyAzureAppService.Entities.DTOS;

namespace MyAzureAppService.Services
{
	public class BackupService : IBackupService
	{
        private readonly BlobContainerClient _blobContainerClient;

		public BackupService(string accountName, string containerName, string connectionString=null)
		{
            if(connectionString==null)
            {
                // Autenticar automáticamente con la identidad administrada
                var credential = new DefaultAzureCredential();

                // Crear un cliente de BlobServiceClient para acceder al contenedor
                var blobServiceClient = new BlobServiceClient(new Uri($"https://{accountName}.blob.core.windows.net"), credential);
                _blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            }
            else
            {
                var blobServiceClient = new BlobServiceClient(connectionString);
                _blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);
            }
        }

        public async Task<BlobDownloadInfo> GetBlob(string fileName)
        {
            try
            {
                // Intenta obtener un cliente de blob con el nombre dado
                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);

                if (!await blobClient.ExistsAsync())
                    return null;

                return await blobClient.DownloadAsync();
            }
            catch(Exception ex)
            {
               throw;
            }
        }

        public async Task<dynamic> GetBlobs()
        {
            try
            {
                var blobs = new List<BlobItemResponseDTO>();

                await foreach (var blob in _blobContainerClient.GetBlobsAsync())
                {
                    blobs.Add(new BlobItemResponseDTO(blob));
                }
                return ExceptionLib.Response.Successful(blobs);
            }
            catch (Exception ex)
            {
                return ExceptionLib.Response.WithErrorException(ex);
            }
        }

        public async Task<dynamic> HandleEventBlob(BlobEventDTO blobEvent)
        {
            try
            {
                
                return ExceptionLib.Response.Successful(blobEvent);
            }
            catch (Exception ex)
            {
                return ExceptionLib.Response.WithErrorException(ex);
            }
        }

        public async Task<dynamic> RegisterBlob(FileDTO fileDTO)
        {
            try
            {
                // Intenta obtener un cliente de blob con el nombre dado
                BlobClient blobClient = _blobContainerClient.GetBlobClient(fileDTO.FileName);

                // Verifica si el blob existe utilizando ExistsAsync
                if (await blobClient.ExistsAsync())
                    return ExceptionLib.Response.UnSuccessful($"File/blob with name {fileDTO.FileName} already exists");

                // Nombre del blob que deseas asignar (puedes usar el nombre original del archivo o cualquier otro nombre único)
                string nombreBlob = fileDTO.FileName + Path.GetExtension(fileDTO.FormFile.FileName);

                // Obtener los datos del archivo desde el objeto FormFile
                using (var ms = new MemoryStream())
                {
                    await fileDTO.FormFile.CopyToAsync(ms);
                    ms.Position = 0;

                    await blobClient.UploadAsync(ms, false);
                }

                return ExceptionLib.Response.Successful(true);
            }
            catch(Exception ex)
            {
                return ExceptionLib.Response.WithErrorException(ex);
            }
        }
    }
}

