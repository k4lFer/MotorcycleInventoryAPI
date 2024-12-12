using DataTransferLayer.Object;
using Service.Generic;

namespace Service.ServiceObject
{
    public class SoBrand : SoGeneric<DtoBrand, object>
    {
        public SoBrand() { body.other = null; }
    }
}
