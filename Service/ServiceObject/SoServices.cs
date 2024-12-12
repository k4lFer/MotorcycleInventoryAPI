using DataTransferLayer.Object;
using Service.Generic;

namespace Service.ServiceObject
{
    public class SoServices : SoGeneric<DtoService, object>
    {
        public SoServices() { body.other = null; }
    }
}

