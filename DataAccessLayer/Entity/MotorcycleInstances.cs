using DataAccessLayer.Generic;

namespace DataAccessLayer.Entity
{
    public class MotorcycleInstances : DateGeneric
    {
        public Guid id { get; set; }
        public Guid motorcycleId { get; set; }
        public string vin { get; set; }
    }
}
