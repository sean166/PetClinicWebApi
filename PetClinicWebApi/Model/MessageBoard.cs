using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Model
{
    public class MessageBoard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string MessageBoardId { get; set; }
        public string Topic { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public int Likes { get; set; }
        public DateTime CreatedTime { get; set; }
        public List<string> RepliedMessages { get; set; }
        public IEnumerable<MessageBoard> MessageBoards { get; set; }
    }
}
