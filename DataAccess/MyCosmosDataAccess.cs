using System;
using Azure.Identity;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Cosmos;

namespace MyAzureAppService.DataAccess
{
	public class MyCosmosDataAccess : IMyCosmosDataAccess
	{
        private readonly Lazy<Task<DatabaseResponse>> _database;

        public MyCosmosDataAccess(string endpoint, string databaseName, string key=null)
        {

            CosmosClient client = null;

            //en caso no venga la key, tratamos de conectar con Identidad administrada
            if (string.IsNullOrEmpty(key))
                client = new CosmosClient(endpoint, new DefaultAzureCredential());
            else
                client = new CosmosClient(endpoint, key);

            //inicializacion lazy para que solo se ejecute la instancia cuando se requiera el valor
            _database = new Lazy<Task<DatabaseResponse>>(async () =>
            {
                return await client.CreateDatabaseIfNotExistsAsync(databaseName, 1000);
            });

        }

        public async Task<DatabaseResponse> GetDatabaseAsync()
        {
            return await _database.Value;
        }
    }
}

