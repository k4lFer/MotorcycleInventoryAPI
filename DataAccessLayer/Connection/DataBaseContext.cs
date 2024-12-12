using DataTransferLayer.OtherObject;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Connection
{
    public partial class DataBaseContext : DbContext
    {
        public DataBaseContext()
        {
            Automapper.Start();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                string connectionString = DtoAppSettings.connetionStringsMySql;
                ServerVersion serverVersion = new MySqlServerVersion(new Version(11, 5, 2));
                optionsBuilder.UseMySql(connectionString, serverVersion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureModel(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}