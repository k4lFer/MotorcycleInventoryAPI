using DataAccessLayer.Query;

namespace BusinessLayer.Business.Sale
{
    public partial class BusinessSales
    {
        private QSales qSales = new();
        private QOwner qOwner = new();
        private QUser qUser = new();
        private QSalesDetails qSalesDetails = new();
        private QMotorcycle qmotorcycle = new();
        private QSalesService qSalesService = new();
        private QService qService = new();
    }
}

