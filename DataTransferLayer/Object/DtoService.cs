using DataTransferLayer.Generic;

namespace DataTransferLayer.Object
{
    public class DtoService : DtoDateGeneric
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
    }
}

