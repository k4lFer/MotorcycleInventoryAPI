using DataTransferLayer.Generic;

namespace DataTransferLayer.Object
{
    public class DtoBrand : DtoDateGeneric
    {
        public Guid id { get; set; }
        public string name { get; set; }
       // public string description { get; set; } 
        public string ruc { get; set; }
    }
}

