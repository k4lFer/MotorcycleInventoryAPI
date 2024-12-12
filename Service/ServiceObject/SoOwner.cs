using DataTransferLayer.Object;
using Service.Generic;

namespace Service.ServiceObject
{
    public class SoOwner : SoGeneric<DtoOwner, object>
    {
        public SoOwner() { body.other = null; }
    }
}
