using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Query
{
    public class QSales
    {
        public async Task<int> createAsync (DtoSales dtoSales)
        {
            using DataBaseContext dbc = new();
            Sales? sales = Automapper.mapper.Map<Sales>(dtoSales);
            await dbc.Sales.AddAsync(sales);
            return await dbc.SaveChangesAsync();
        }
        public int create (DtoSales dtoSales)
        {
            using DataBaseContext dbc = new();
            Sales? sales = Automapper.mapper.Map<Sales>(dtoSales);
             dbc.Sales.Add(sales);
            return dbc.SaveChanges();
        }
        public async Task<int> updateAsync(DtoSales dtoSales)
        {
            using DataBaseContext dbc = new();
            Sales? sales = Automapper.mapper.Map<Sales>(dtoSales);
            dbc.Sales.Update(sales);
            return await dbc.SaveChangesAsync();
        }
        public int update(DtoSales dtoSales)
        {
            using DataBaseContext dbc = new();
            Sales? sales = Automapper.mapper.Map<Sales>(dtoSales);
            dbc.Sales.Update(sales);
            return dbc.SaveChanges();
        }
        public List<DtoSales> getByUserId(Guid id)
        {
            using DataBaseContext dbc = new();
            var salesH = dbc.Sales
               /* .Include(s => s.ParentUser)
                .Include(s => s.ParentOwner)*/
                .Include(s => s.ChildSalesMotorcycles)
                    .ThenInclude(sd => sd.ParentMotorcycle)
                .Include(s => s.ChildMotorcycleServices)
                    .ThenInclude(sd => sd.ParentService)
                .Where(s => s.userId == id)
                .OrderByDescending(s => s.date)
                .ToList();
            return Automapper.mapper.Map<List<DtoSales>>(salesH);
        }

        public DtoSales? getById(Guid saleId)
        {
            using DataBaseContext dbc = new();
            Sales? sale = dbc.Sales
                .Include(s => s.ChildSalesMotorcycles)
                .ThenInclude(sd => sd.ParentMotorcycle)
                .Include(s => s.ChildMotorcycleServices)
                .ThenInclude(sd => sd.ParentService)
                .OrderByDescending(s => s.date)
                .FirstOrDefault(s => s.id == saleId);
            return Automapper.mapper.Map<DtoSales>(sale);
        }

        public DtoSales? getByOwnerId(Guid ownerId)
        {
            using DataBaseContext dbc = new();
            Sales? sale = dbc.Sales
                .Include(s => s.ChildSalesMotorcycles)
                .ThenInclude(sd => sd.ParentMotorcycle)
                .Include(s => s.ChildMotorcycleServices)
                .ThenInclude(sd => sd.ParentService)
                .OrderByDescending(s => s.date)
                .FirstOrDefault(s => s.ownerId == ownerId);
            return Automapper.mapper.Map<DtoSales>(sale);
        }

        public List<DtoSales> getAll()
        {
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<List<DtoSales>>(dbc.Sales
                /*.Include(s => s.ParentUser)
                .Include(s => s.ParentOwner)*/
                .Include(s => s.ChildSalesMotorcycles)
                .Include(s => s.ChildMotorcycleServices)
                .ToList());
        }
    }
}
