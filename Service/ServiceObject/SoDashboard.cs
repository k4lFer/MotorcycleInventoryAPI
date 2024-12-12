using DataTransferLayer.Object;
using Service.Generic;

namespace Service.ServiceObject
{
    public class SoDashboard : SoGeneric<DtoStatistics, Object>
    {
        public SoDashboard()
        {
            body.other = null;
        }
    }
}

