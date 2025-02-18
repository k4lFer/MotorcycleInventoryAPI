using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Query
{
    public class QSalesMotorcycles
    {
        public ICollection<DtoSalesMotorcycles> getBySale(Guid id)
        {
            using DataBaseContext dbc = new();
            var salesDetails = dbc.SalesDetails
                .AsNoTracking() // No rastrear entidades
                .Include(s => s.ParentMotorcycle)
                .Where(s => s.saleId == id)
                .ToList();
            return Automapper.mapper.Map<ICollection<DtoSalesMotorcycles>>(salesDetails);
        }
        public async Task<int> createAsync(DtoSalesMotorcycles dtoSalesDetails)
        {
            using DataBaseContext dbc = new();
            Entity.SalesMotorcycles? salesDetails = Automapper.mapper.Map<Entity.SalesMotorcycles>(dtoSalesDetails);

           /* // Verificar rastreo previo
            var existingEntity = dbc.SalesDetails.Local.FirstOrDefault(e => e.id == salesDetails.id);
            if (existingEntity != null)
            {
                dbc.Entry(existingEntity).State = EntityState.Detached;
            }*/

            await dbc.SalesDetails.AddAsync(salesDetails);
            return await dbc.SaveChangesAsync();
        }
        public int create(DtoSalesMotorcycles dtoSalesDetails)
        {
            using DataBaseContext dbc = new();
            Entity.SalesMotorcycles? salesDetails = Automapper.mapper.Map<Entity.SalesMotorcycles>(dtoSalesDetails);
            
            dbc.SalesDetails.Add(salesDetails);
            return dbc.SaveChanges();
        }
    }
}
