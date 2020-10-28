using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Model
{
    public class Pet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string PetId { get; set; }
        public string PetName { get; set; }
        public string PetGender { get; set; }
        public string PetBreed { get; set; }
        public string PetDesc { get; set; }
        public double Weight { get; set; }
        public double Age { get; set; }
        
    }
}
