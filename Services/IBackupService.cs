using System;
using Azure.Storage.Blobs.Models;
using MyAzureAppService.Entities.DTOS;

namespace MyAzureAppService.Services
{
	public interface IBackupService
	{
		/// <summary>
		/// Registra un blob en contenedor
		/// </summary>
		/// <returns></returns>
		Task<dynamic> RegisterBlob(FileDTO fileDTO);

		/// <summary>
		/// Obtiene informacion de blobs en contenedor
		/// </summary>
		/// <returns></returns>
		Task<dynamic> GetBlobs();

		/// <summary>
		/// obtiene informacion de descarga de blob en contenedor
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<BlobDownloadInfo> GetBlob(string fileName);

		/// <summary>
		/// Maneka la recepcion de un evento de blob
		/// </summary>
		/// <param name="blobEvent"></param>
		/// <returns></returns>
		Task<dynamic> HandleEventBlob(BlobEventDTO blobEvent);
	}
}

