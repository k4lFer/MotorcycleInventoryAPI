using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;

namespace DataAccessLayer.Query
{
    public class QAuthentication
    {
        public bool ExistByEmailUser(string email)
        {
            using DataBaseContext dbc = new();
            return dbc.Users.Any(w => w.email == email);
        }

        public bool ExistByPhoneNumberUser(string phoneNumber)
        {
            using DataBaseContext dbc = new();
            return dbc.Users.Any(w => w.phoneNumber == phoneNumber);
        }
        
        public DtoUser? GetByEmailUser(string email)
        {
            using DataBaseContext dbc = new();
            User? user = dbc.Users.FirstOrDefault(w => w.email == email);
            return Automapper.mapper.Map<DtoUser>(user);
        }

        public DtoUser? GetByPhoneNumberUser(string phoneNumber)
        {
            using DataBaseContext dbc = new();
            User? user = dbc.Users.FirstOrDefault(w => w.phoneNumber == phoneNumber);
            return Automapper.mapper.Map<DtoUser>(user);
        }
        
        //==================================================================================
        public bool ExistByUsernameOwner(string username)
        {
            using DataBaseContext dbc = new();
            return dbc.Owners.Any(w => w.username == username);
        }
        
        public bool ExistByEmailOwner(string email)
        {
            using DataBaseContext dbc = new();
            return dbc.Owners.Any(w => w.email == email);
        }
        
        public bool ExistByDniOwner(string dni)
        {
            using DataBaseContext dbc = new();
            return dbc.Owners.Any(w => w.dni == dni);
        }
        
        public bool ExistByRucOwner(string ruc)
        {
            using DataBaseContext dbc = new();
            return dbc.Owners.Any(w => w.ruc == ruc);
        }

        public bool ExistByPhoneNumberOwner(string phoneNumber)
        {
            using DataBaseContext dbc = new();
            return dbc.Owners.Any(w => w.phoneNumber == phoneNumber);
        }
        
        public DtoOwner? SearchByUsernameOwner(string username)
        {
            using DataBaseContext dbc = new();
            Owner? owner = dbc.Owners.FirstOrDefault(w => w.username == username);
            return Automapper.mapper.Map<DtoOwner>(owner);
        }
        
        public DtoOwner? GetByUsernameOwner(string username)
        {
            using DataBaseContext dbc = new();
            Owner? owner = dbc.Owners.FirstOrDefault(w => w.username == username);
            return Automapper.mapper.Map<DtoOwner>(owner);
        }
        
        public DtoOwner? GetByEmailOwner(string email)
        {
            using DataBaseContext dbc = new();
            Owner? owner = dbc.Owners.FirstOrDefault(w => w.email == email);
            return Automapper.mapper.Map<DtoOwner>(owner);
        }
        
        public DtoOwner? GetByDniOwner(string dni)
        {
            using DataBaseContext dbc = new();
            Owner? owner = dbc.Owners.FirstOrDefault(w => w.dni == dni);
            return Automapper.mapper.Map<DtoOwner>(owner);
        }
        
        public DtoOwner? GetByRucOwner(string ruc)
        {
            using DataBaseContext dbc = new();
            Owner? owner = dbc.Owners.FirstOrDefault(w => w.ruc == ruc);
            return Automapper.mapper.Map<DtoOwner>(owner);
        }
        
        public DtoOwner? GetByPhoneNumberOwner(string phoneNumber)
        {
            using DataBaseContext dbc = new();
            Owner? owner = dbc.Owners.FirstOrDefault(w => w.phoneNumber == phoneNumber);
            return Automapper.mapper.Map<DtoOwner>(owner);
        }
    }
}
