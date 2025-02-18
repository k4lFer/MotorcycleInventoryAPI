using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Query
{
    public class QMotorcycle
    {
        public int create(DtoMotorcycle dto)
        {
            using DataBaseContext dbc = new();
            Motorcycle? motorcycle = Automapper.mapper.Map<Motorcycle>(dto);
            dbc.Motorcyles.Add(motorcycle);
            return dbc.SaveChanges();

        }
        public int update(DtoMotorcycle dto) 
        {
            using DataBaseContext dbc = new();
            Motorcycle? motorcycle = Automapper.mapper.Map<Motorcycle>(dto);
            dbc.Motorcyles.Update(motorcycle);
            return dbc.SaveChanges();
        }
        public async Task<int> updateAsync(DtoMotorcycle dto) 
        {
            using DataBaseContext dbc = new();
            Motorcycle? motorcycle = Automapper.mapper.Map<Motorcycle>(dto);
            dbc.Motorcyles.Update(motorcycle);
            return await dbc.SaveChangesAsync();
        }
        public DtoMotorcycle getById(Guid id)
        {
            using DataBaseContext dbc = new();
            Motorcycle? motorcycle = dbc.Motorcyles
                .Include(m => m.ParentBrands)
                .Include(m => m.ParentTypes)
                .FirstOrDefault(m => m.id == id);
            return Automapper.mapper.Map<DtoMotorcycle>(motorcycle);
        }

       /* public DtoMotorcycle getByVin(string vin)
        {
            using DataBaseContext dbc = new();
            Motorcycle? motorcycle = dbc.Motorcyles
                .Include(m => m.ParentBrands)
                .Include(m => m.ParentTypes)
                .FirstOrDefault(m => m.vin == vin);
            return Automapper.mapper.Map<DtoMotorcycle>(motorcycle);
        }*/
        
        public async Task<DtoMotorcycle> getByIdAsync(Guid id)
        {
            using DataBaseContext dbc = new();
            Motorcycle? motorcycle = await dbc.Motorcyles
                .Include(m => m.ParentBrands)
                .Include(m => m.ParentTypes)
                .FirstOrDefaultAsync(m => m.id == id);
            return Automapper.mapper.Map<DtoMotorcycle>(motorcycle);
        }
        
        public List<DtoMotorcycle> getAll()
        {
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<List<DtoMotorcycle>>(dbc.Motorcyles
                .Include(m => m.ParentBrands)
                .Include(m => m.ParentTypes)
                .OrderBy(m => m.createdAt).ToList());
        }
        public int UpdateStatusBasedOnStock()
        {
            using DataBaseContext dbc = new();
            var motorcycles = dbc.Motorcyles
                .Where(m => m.quantity== 0 && m.status !=  StatusEnum.not_available)
                .ToList();

            foreach (var motorcycle in motorcycles)
            {
                motorcycle.status = StatusEnum.not_available;
            }

            return dbc.SaveChanges();
        }
    }
}
