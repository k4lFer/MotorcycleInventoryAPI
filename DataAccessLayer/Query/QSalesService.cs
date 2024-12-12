using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Query
{
    public class QSalesService
    {
        public ICollection<DtoSalesService> getBySale(Guid id)
        {
            using DataBaseContext dbc = new();
            var saleService = dbc.SalesServices
                .Include(s => s.ParentService)
                .Where(s => s.saleId == id)
                .ToList();
            return Automapper.mapper.Map<ICollection<DtoSalesService>>(saleService);
        }
        public async Task<int> createAsync(DtoSalesService dtoSalesService)
        {
            using DataBaseContext dbc = new();
            SalesService? salesService = Automapper.mapper.Map<SalesService>(dtoSalesService);

            await dbc.SalesServices.AddAsync(salesService);
            return await dbc.SaveChangesAsync();
        }
        
        public int create(DtoSalesService dtoSalesService)
        {
            using DataBaseContext dbc = new();
            SalesService? salesService = Automapper.mapper.Map<SalesService>(dtoSalesService);
            dbc.SalesServices.Add(salesService);
            return dbc.SaveChanges();
        }
    }
}
