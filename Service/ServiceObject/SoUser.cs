using DataTransferLayer.Object;
using Service.Generic;

namespace Service.ServiceObject
{
    public class SoUser : SoGeneric<DtoUser, object>
    {
        public SoUser() { body.other = null; }
    }
}
