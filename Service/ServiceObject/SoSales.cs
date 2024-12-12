using DataTransferLayer.Object;
using Service.Generic;

namespace Service.ServiceObject{
    public class SoSales : SoGeneric<DtoSales, object>
    {
        public SoSales() { body.other = null; }
    }
}

