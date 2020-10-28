using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetClinicWebApi.Model
{
    public class PetClinicDatabaseSettings:IPetClinicDatabaseSettings
    {
        public string PetCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IPetClinicDatabaseSettings
    {
        string PetCollectionName { set; get; }
        string ConnectionString { set; get; }
        string DatabaseName { set; get; }
    }
}
