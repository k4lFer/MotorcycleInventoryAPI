using DataAccessLayer.Connection;
using DataAccessLayer.Entity;
using DataTransferLayer.Object;

namespace DataAccessLayer.Query
{
    public class QBrand
    {
        public int create(DtoBrand dtoBrand)
        {
            using DataBaseContext dbc = new();
            Brand? brand = Automapper.mapper.Map<Brand>(dtoBrand);
            dbc.Brands.Add(brand);
            return dbc.SaveChanges();
        }
        public int update(DtoBrand dtoBrand)
        {
            using DataBaseContext dbc = new();
            Brand? brand = Automapper.mapper.Map<Brand>(dtoBrand);
            dbc.Brands.Update(brand);
            return dbc.SaveChanges();
        }
        public List<DtoBrand> getAll()
        {
            using DataBaseContext dbc = new();
            return Automapper.mapper.Map<List<DtoBrand>>(dbc.Brands);
        }
        public DtoBrand getById(Guid id) 
        {
            using DataBaseContext dbc = new();
            Brand? brand = dbc.Brands.FirstOrDefault(b => b.id == id);
            return Automapper.mapper.Map<DtoBrand>(brand);
        }
        
    }
}
