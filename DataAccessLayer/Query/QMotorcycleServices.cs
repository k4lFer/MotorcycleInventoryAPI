using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Query
{
    public class QMotorcycleServices
    {
        public ICollection<DtoMotorcycleServices> getBySale(Guid id)
        {
            using DataBaseContext dbc = new();
            var saleService = dbc.SalesServices
                .Include(s => s.ParentService)
                .Where(s => s.saleId == id)
                .ToList();
            return Automapper.mapper.Map<ICollection<DtoMotorcycleServices>>(saleService);
        }
        public async Task<int> createAsync(DtoMotorcycleServices dtoSalesService)
        {
            using DataBaseContext dbc = new();
            MotorcycleServices? salesService = Automapper.mapper.Map<MotorcycleServices>(dtoSalesService);

            await dbc.SalesServices.AddAsync(salesService);
            return await dbc.SaveChangesAsync();
        }
        
        public int create(DtoMotorcycleServices dtoSalesService)
        {
            using DataBaseContext dbc = new();
            MotorcycleServices? salesService = Automapper.mapper.Map<MotorcycleServices>(dtoSalesService);
            dbc.SalesServices.Add(salesService);
            return dbc.SaveChanges();
        }
    }
}
