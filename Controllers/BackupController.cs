using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MyAzureAppService.Entities.DTOS;
using MyAzureAppService.Hubs;
using MyAzureAppService.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MyAzureAppService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class BackupController : Controller
	{
		private readonly IBackupService _backupService;
		private readonly IHubContext<BlobNotifyHub, IBlobNotifyClient> _notifyBlobClient;

		public BackupController(IBackupService backupService, IHubContext<BlobNotifyHub, IBlobNotifyClient> notifyBlobClient)
		{
			_backupService = backupService;
			_notifyBlobClient = notifyBlobClient;
		}

		/// <summary>
		/// Devuelve lista de blobs en container server azure
		/// </summary>
		/// <returns></returns>
		[Route(nameof(BackupController.GetAll)), HttpGet]
		public async Task<dynamic> GetAll()
		{
			return await _backupService.GetBlobs();
		}

		/// <summary>
		/// Registra un blob en container server azure
		/// </summary>
		/// <param name="fileDTO"></param>
		/// <returns></returns>
		[Route(nameof(BackupController.UploadFile)), HttpPost]
		public async Task<dynamic> UploadFile([FromForm]FileDTO fileDTO)
		{
			return await _backupService.RegisterBlob(fileDTO);
		}

		/// <summary>
		/// Descarga archivo blob de container server azure
		/// </summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
        [Route(nameof(BackupController.DownloadFile)), HttpGet]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
			try
			{
                var response = await _backupService.GetBlob(fileName);
				if (response == null)
					return ExceptionLib.Response.UnSuccessful($"File blob {fileName} not exists");

                return File(response.Content, response.ContentType, fileName);
            }
            catch(Exception ex)
			{
				return null;
			}

        }

		/// <summary>
		/// Webhook para recibir eventos de blob desde azure (AZ eventgrid)
		/// </summary>
		/// <param name="eventBlob"></param>
		/// <returns></returns>
        [Route(nameof(BackupController.NewEventBlobWebHook)), HttpPost]
        public async Task<dynamic> NewEventBlobWebHook([FromBody]object request)
        {
            try
			{
                var eventGridEvents = JsonConvert.DeserializeObject<BlobEventDTO[]>(request.ToString());

                // Verificamos si el evento es de validación
                if (eventGridEvents.Count() == 1 && eventGridEvents[0].EventType == "Microsoft.EventGrid.SubscriptionValidationEvent")
                {
                    var data = eventGridEvents.FirstOrDefault().Data as JObject;
                    var eventData = data.ToObject<SubscriptionValidationEventData>();

                    // Es un evento de validación, respondemos con el código de validación
                    return Ok(new { validationResponse = eventData.ValidationCode });
                }

                // Si no es un evento de validación, procesa el evento normalmente
                foreach (var azEvent in eventGridEvents)
                {
					// Procesa cada evento azEvent aquí con SignalR
					await _notifyBlobClient.Clients.All.ReceiveNotification(azEvent);
                }

                return Ok();
            }
            catch(Exception ex)
			{
				return BadRequest();
			}
            
		} 
    }
}

