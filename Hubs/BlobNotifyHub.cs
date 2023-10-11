using System;
using Microsoft.AspNetCore.SignalR;

namespace MyAzureAppService.Hubs
{
	public class BlobNotifyHub : Hub<IBlobNotifyClient>
	{
		//si deseas puedes sobreescribir los metodos
	}
}

