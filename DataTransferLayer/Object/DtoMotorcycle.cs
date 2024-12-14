using DataTransferLayer.Generic;
using DataTransferLayer.OtherObject;

namespace DataTransferLayer.Object
{
    public class DtoMotorcycle : DtoDateGeneric
    {
        public Guid id { get; set; }
        public Guid? typeId { get; set; }
        public Guid? brandId { get; set; }
        public string name { get; set; }
        public string displacement { get; set; }
        public string vin { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public StatusEnum status { get; set; }

        #region Parent
        public DtoTypes ParentType { get; set; } = null;
        public DtoBrand ParentBrand { get; set; } = null;
        #endregion
    }
}

