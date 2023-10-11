using System;
using Microsoft.Azure.Cosmos;

namespace MyAzureAppService.DataAccess
{
	public interface IMyCosmosDataAccess
	{
		Task<DatabaseResponse> GetDatabaseAsync();
	}
}

