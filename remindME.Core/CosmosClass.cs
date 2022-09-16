using remindME.Core.Cosmos;

namespace remindME.Core
{
    public static class CosmosClass
    {

        /// <summary>
        /// Creates a Cosmos DB database and a container with the specified partition key. 
        /// </summary>
        /// <returns></returns>
        public static async Task<CosmosDbService> InitializeCosmosClientInstanceAsync(string connectionString, string db, string container)
        {

            Microsoft.Azure.Cosmos.CosmosClient client = new Microsoft.Azure.Cosmos.CosmosClient(connectionString);
            CosmosDbService cosmosDbService = new CosmosDbService(client, db, container);
            Microsoft.Azure.Cosmos.DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(db);
            await database.Database.CreateContainerIfNotExistsAsync(container, "/datetime");

            return cosmosDbService;
        }
    }
}
