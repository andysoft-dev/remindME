namespace remindME.Core.Cosmos;

    using System.Collections.Generic;
    using System.Threading.Tasks;
    using remindME.Core.Models;

    public interface ICosmosDbService
    {
        Task<IEnumerable<Reminder>> GetItemsAsync(string query);
        Task<Reminder> GetItemAsync(string id);
        Task AddItemAsync(Reminder item);
        Task UpdateItemAsync(string id, Reminder item);
        Task DeleteItemAsync(string id);
    }
