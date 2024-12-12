using DataTransferLayer.Object;
using Service.Generic;

namespace Service.ServiceObject
{
    public class SoMotorcycle :SoGeneric<DtoMotorcycle, object>
    {
        public SoMotorcycle() { body.other = null; }
    }
}
