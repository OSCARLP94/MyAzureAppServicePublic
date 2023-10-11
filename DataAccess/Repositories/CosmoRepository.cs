using System;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Cosmos;

namespace MyAzureAppService.DataAccess.Repositories
{
	public class CosmoRepository<T> : ICosmoRepository<T>
		where T: class
	{
        private readonly IMyCosmosDataAccess _cosmoDataAccess;
        private Container _container;

		public CosmoRepository(IMyCosmosDataAccess cosmoDataAccess, string containerId, string partitionKey)
		{
            try
            {
                _cosmoDataAccess = cosmoDataAccess;
                InitializeAsync(containerId, partitionKey).Wait();
            }
            catch(Exception ex)
            {
                // Registrar la excepción en Application Insights
                TelemetryClient telemetry = new TelemetryClient();
                telemetry.TrackException(ex);

                throw;
            }
        }

        private async Task InitializeAsync(string containerId, string partitionKey)
        {
            var cosmosDataBase = await _cosmoDataAccess.GetDatabaseAsync();
            _container = await cosmosDataBase.Database.CreateContainerIfNotExistsAsync(
            id: containerId,
            partitionKeyPath: partitionKey);
        }

        public async Task<ICollection<T>> ListData()
        {
            List<T> items = new List<T>();

            using FeedIterator<T> resultSetIterator = _container.GetItemQueryIterator<T>("SELECT * FROM c");
            while (resultSetIterator.HasMoreResults)
            {
                FeedResponse<T> response = await resultSetIterator.ReadNextAsync();
                items.AddRange(response);
            }

            return items;
        }

        public async Task<T> Register(T item, string partitionKey)
        {
            ItemResponse<T> response = await _container.CreateItemAsync(item, new PartitionKey(partitionKey));

            return response.Resource;
        }
    }
}

