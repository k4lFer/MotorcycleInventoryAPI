using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;

namespace DataAccessLayer.Query
{
    public class QTypes
    {
        public int create(DtoTypes dtoTypes)
        {
            using DataBaseContext dbc = new();
            Types? types = Automapper.mapper.Map<Types>(dtoTypes);
            dbc.Types.Add(types);
            return dbc.SaveChanges();
        }
        public int update(DtoTypes dtoTypes)
        {
            using DataBaseContext dbc = new();
            Types? types = Automapper.mapper.Map<Types>(dtoTypes);
            dbc.Types.Update(types);
            return dbc.SaveChanges();
        }
        public List<DtoTypes> getAll()
        {
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<List<DtoTypes>>(dbc.Types);
        }
        public DtoTypes getById(Guid id)
        {
            using DataBaseContext dbc = new();
            Types? types = dbc.Types.FirstOrDefault(b => b.id == id);
            return Automapper.mapper.Map<DtoTypes>(types);
        }
    }
}
