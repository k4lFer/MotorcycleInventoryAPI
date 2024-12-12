using DataTransferLayer.Object;
using Service.Generic;

namespace Service.ServiceObject
{
    public class SoTypes : SoGeneric<DtoTypes, Object>
    {
        public SoTypes() { body.other = null; }
    }
}

