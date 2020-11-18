using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Model
{
    public class QA
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string QAId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
