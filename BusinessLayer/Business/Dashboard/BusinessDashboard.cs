using BusinessLayer.Generic;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;

namespace BusinessLayer.Business.Dashboard
{
    public partial class BusinessDashboard : BusinessGeneric
    {
        public (DtoMessage, DtoStatistics) GetStatistics()
        {
            
            DtoStatistics dtoStatistics;
            dtoStatistics = qStatistics.GetStatistics();
            if (dtoStatistics != null)
            {
                _message.Success();
            }
            return (_message, dtoStatistics);
        }
    }
}

