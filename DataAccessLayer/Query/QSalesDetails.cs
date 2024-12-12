using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Query
{
    public class QSalesDetails
    {
        public ICollection<DtoSalesDetails> getBySale(Guid id)
        {
            using DataBaseContext dbc = new();
            var salesDetails = dbc.SalesDetails
                .AsNoTracking() // No rastrear entidades
                .Include(s => s.ParentMotorcyle)
                .Where(s => s.saleId == id)
                .ToList();
            return Automapper.mapper.Map<ICollection<DtoSalesDetails>>(salesDetails);
        }
        public async Task<int> createAsync(DtoSalesDetails dtoSalesDetails)
        {
            using DataBaseContext dbc = new();
            SalesDetails? salesDetails = Automapper.mapper.Map<SalesDetails>(dtoSalesDetails);

           /* // Verificar rastreo previo
            var existingEntity = dbc.SalesDetails.Local.FirstOrDefault(e => e.id == salesDetails.id);
            if (existingEntity != null)
            {
                dbc.Entry(existingEntity).State = EntityState.Detached;
            }*/

            await dbc.SalesDetails.AddAsync(salesDetails);
            return await dbc.SaveChangesAsync();
        }
        public int create(DtoSalesDetails dtoSalesDetails)
        {
            using DataBaseContext dbc = new();
            SalesDetails? salesDetails = Automapper.mapper.Map<SalesDetails>(dtoSalesDetails);
            
            dbc.SalesDetails.Add(salesDetails);
            return dbc.SaveChanges();
        }
    }
}
