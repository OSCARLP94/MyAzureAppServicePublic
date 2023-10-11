using System;
using MyAzureAppService.Entities.DTOS;

namespace MyAzureAppService.Hubs
{
	public interface IBlobNotifyClient
	{
		Task ReceiveNotification(BlobEventDTO eventDTO);
	}
}

