using System.Collections;

namespace DataTransferLayer.Object
{
    public class DtoSales
    {
        public Guid id { get; set; }
        public Guid? userId { get; set; }
        public Guid? ownerId { get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
        public DateTime date { get; set; }

        #region Childs
        public ICollection<DtoSalesMotorcycles> ChildDtoSalesMotorcycles { get; set; }  = new List<DtoSalesMotorcycles>();
        public ICollection<DtoMotorcycleServices> ChildDtoMotorcycleServices { get; set; } = new List<DtoMotorcycleServices>(); 
        #endregion
    }
}

