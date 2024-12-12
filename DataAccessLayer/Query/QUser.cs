using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;

namespace DataAccessLayer.Query
{
    public class QUser
    {
        public int Register(DtoUser dto){
            using DataBaseContext dbc = new();
            dbc.Add(Automapper.mapper.Map<User>(dto));
            return dbc.SaveChanges();
        }
        
        public DtoUser? GetById(Guid id)
        {
            using DataBaseContext dbc = new();
            User? user = dbc.Users.FirstOrDefault(w => w.id == id);
            return Automapper.mapper.Map<DtoUser>(user);
        }
        public DtoUser? GetByDocumentNumber(string documentNumber)
        {
            using DataBaseContext dbc = new();
            User? user = dbc.Users.FirstOrDefault(w => w.documentNumber == documentNumber);
            return Automapper.mapper.Map<DtoUser>(user);
        }

        public List<string> getAllByDocumentNumber(string documentNumber)
        {
            using DataBaseContext dbc = new();
            var documents = dbc.Users
                .Where(user => user.documentNumber.Contains(documentNumber))
                .Select(user => user.documentNumber)
                .ToList();
            return documents;
        }
        
        public int Update(DtoUser dto){
            using DataBaseContext dbc = new();
            User user = Automapper.mapper.Map<User>(dto);
            if (user != null)
            {
                dbc.Users.Update(user);
                return dbc.SaveChanges();
            }
            return 0;
        }
        
        public int UpdatePassword(DtoUser dto){
            using DataBaseContext dbc = new();
            User user = Automapper.mapper.Map<User>(dto);
            if (user != null)
            {
                dbc.Users.Update(user);
                return dbc.SaveChanges();
            }
            return 0;
        }
        
        public List<DtoUser> GetAll()
        {
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<List<DtoUser>>(dbc.Users.ToList());
        }
        
        public DtoUser getById(Guid id){
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<DtoUser>(dbc.Users.Find(id));
        }
        
        public int DeleteOnCascade(Guid id)
        {
            using DataBaseContext dbc = new();
            User? user = dbc.Users.FirstOrDefault(u => u.id == id);
            if (user != null)
            {
                dbc.Users.Remove(user);
                return dbc.SaveChanges();
            }
            return 0;
        }
        
        public bool ExistByIdUser(Guid id)
        {
            using DataBaseContext dbc = new();
            return dbc.Users.Any(w => w.id == id);
        }
        
        public async Task<bool> ExistByIdUserAsync(Guid id)
        {
            await using DataBaseContext dbc = new();
            return dbc.Users.Any(w => w.id == id);
        }
    }
}
