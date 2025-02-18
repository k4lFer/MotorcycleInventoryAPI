using DataAccessLayer.Query;

namespace BusinessLayer.Business.Sale
{
    public partial class BusinessSales
    {
        private QSales qSales = new();
        private QOwner qOwner = new();
        private QUser qUser = new();
        private QSalesMotorcycles qSalesDetails = new();
        private QMotorcycle qmotorcycle = new();
        private QMotorcycleServices qSalesService = new();
        private QService qService = new();
    }
}

