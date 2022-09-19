using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using remindME.Core.Models;

namespace remindME.Core.Cosmos
{
    public class CosmosDbService : ICosmosDbService
    {
        private Microsoft.Azure.Cosmos.Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(Reminder item)
        {
            await this._container.CreateItemAsync<Reminder>(item);
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

        public async Task<List<Reminder>> GetReminders(DateTime fec)
        {
            List<Reminder> lista = new List<Reminder>();
            using (FeedIterator<Reminder> setIterator = this._container.GetItemLinqQueryable<Reminder>().Where(p => p.datetime < fec && p.sent == false)
                      .ToFeedIterator<Reminder>())
            {
                //Asynchronous query execution
                while (setIterator.HasMoreResults)
                {
                    foreach (var item in await setIterator.ReadNextAsync())
                    {
                        lista.Add(new Reminder { 
                            datetime = item.datetime,
                            id = item.id,
                            message =  item.message,
                            title = item.title,
                            sent = item.sent
                        });
                    }
                }
            }


            return lista;
        }

        public async Task UpdateItemAsync(string id, Reminder item)
        {
            await this._container.UpsertItemAsync<Reminder>(item, new PartitionKey(id));
        }

    }
}


