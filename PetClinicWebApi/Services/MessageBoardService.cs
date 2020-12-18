using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using PetClinicWebApi.Model;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Services
{
    public class MessageBoardService
    {
        private readonly IMongoCollection<MessageBoard> _messageBoard;
        public MessageBoardService(IPetClinicDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _messageBoard = database.GetCollection<MessageBoard>("MessageBoards");
        }
        public async Task<List<MessageBoard>> GetAll()
        {
            return await _messageBoard.Find(q => true).ToListAsync();
        }
        public async Task<MessageBoard> GetMessage(string id)
        {
            return  await _messageBoard.Find(m => m.MessageBoardId == id).FirstOrDefaultAsync();
        }
        public async Task<MessageBoard> CreateAsync(MessageBoard message)
        {
            await _messageBoard.InsertOneAsync(message);
            return message;
        }
        public async Task UpdateAsync(string id, MessageBoard message)
        {
            await _messageBoard.ReplaceOneAsync(s => s.MessageBoardId == id, message);
        }
        public async Task DeleteAsync(string id)
        {
            await _messageBoard.DeleteOneAsync(s => s.MessageBoardId == id);
        }
    }
}
