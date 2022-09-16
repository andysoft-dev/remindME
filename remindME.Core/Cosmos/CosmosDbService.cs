using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using remindME.Core;
using remindME.Core.Models;

namespace remindME.Core.Cosmos
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Reminder item)
        {
            await this._container.CreateItemAsync<Reminder>(item, new PartitionKey(item.datetime));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<Reminder>(id, new PartitionKey(id));
        }

        public async Task<Reminder> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<Reminder> response = await this._container.ReadItemAsync<Reminder>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

        }

        public async Task<IEnumerable<Reminder>> GetItemsAsync(string queryString)
        {
            var query = this._container.GetItemQueryIterator<Reminder>(new QueryDefinition(queryString));
            List<Reminder> results = new List<Reminder>();
            while (query.HasMoreResults)
            {
                var response = await query.ReadNextAsync();

                results.AddRange(response.ToList());
            }

            return results;
        }

        public async Task UpdateItemAsync(string id, Reminder item)
        {
            await this._container.UpsertItemAsync<Reminder>(item, new PartitionKey(id));
        }
    }
}


