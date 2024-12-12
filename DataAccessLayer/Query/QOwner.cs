using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;

namespace DataAccessLayer.Query
{
    public class QOwner
    {
        public async Task<int> Register(DtoOwner dto){
            using DataBaseContext dbc = new();
            dbc.AddAsync(Automapper.mapper.Map<Owner>(dto));
            return await dbc.SaveChangesAsync();
        }
        
        public DtoOwner? GetById(Guid id)
        {
            using DataBaseContext dbc = new();
            Owner? owner = dbc.Owners.FirstOrDefault(w => w.id == id);
            return Automapper.mapper.Map<DtoOwner>(owner);
        }
        
        public int Update(DtoOwner dto){
            using DataBaseContext dbc = new();
            Owner owner = Automapper.mapper.Map<Owner>(dto);
            if (owner != null)
            {
                dbc.Owners.Update(owner);
                return dbc.SaveChanges();
            }
            return 0;
        }
        
        public int UpdatePassword(DtoOwner dto){
            using DataBaseContext dbc = new();
            Owner owner = Automapper.mapper.Map<Owner>(dto);
            if (owner != null)
            {
                dbc.Owners.Update(owner);
                return dbc.SaveChanges();
            }
            return 0;
        }
        
        public List<DtoOwner> GetAll(Guid id)
        {
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<List<DtoOwner>>(dbc.Owners.Where(u => u.id != id).ToList());
        }
        
        public DtoOwner MyProfile(Guid id){
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<DtoOwner>(dbc.Owners.Find(id));
        }
        
        public int DeleteOnCascadeOwner(Guid id)
        {
            using DataBaseContext dbc = new();
            User? user = dbc.Users.SingleOrDefault(u => u.id == id);
            if (user != null)
            {
                dbc.Users.Remove(user);
                return dbc.SaveChanges();
            }
            return 0;
        }
        
        public int UpdatePromoteOrDemote(DtoOwner dto){
            using DataBaseContext dbc = new();
            Owner owner = Automapper.mapper.Map<Owner>(dto);
            if (owner != null)
            {
                dbc.Owners.Update(owner);
                return dbc.SaveChanges();
            }
            return 0;
        }
    }
}
