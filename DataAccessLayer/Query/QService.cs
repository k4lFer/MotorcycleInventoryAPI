using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Query
{
    public class QService
    {
        public int create(DtoService dto){
            using DataBaseContext dbc = new();
            Service? service = Automapper.mapper.Map<Service>(dto);
            dbc.Services.Add(service);
            return dbc.SaveChanges();
        }
        public int update(DtoService dto){
            using DataBaseContext dbc = new();
            Service? service = Automapper.mapper.Map<Service>(dto);
            dbc.Services.Update(service);
            return dbc.SaveChanges();
        }
        public DtoService getById(Guid id){
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<DtoService>(dbc.Services.
            FirstOrDefault(s => s.id == id));
        }
        
        public async Task<DtoService> getByIdAsync(Guid id){
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<DtoService>(dbc.Services.
                FirstOrDefaultAsync(s => s.id == id));
        }
        public List<DtoService> getAll(){
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<List<DtoService>>(dbc.Services.ToList());
        }
    }
}
