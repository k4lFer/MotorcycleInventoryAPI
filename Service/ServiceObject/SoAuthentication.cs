using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;
using Service.Generic;

namespace Service.ServiceObject
{
    public class SoAuthentication : SoGeneric<DtoAuthentication, Tokens>
    {
        public SoAuthentication()
        {
            body.other = new Tokens();
        }
    }
}
