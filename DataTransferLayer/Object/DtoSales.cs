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

        /*#region Parent
        public DtoOwner ParentOwner { get; set; }
        public DtoUser ParentUser { get; set; }
        #endregion*/

        #region Childs
        public ICollection<DtoSalesDetails> ChildDtoSalesDetails { get; set; }  = new List<DtoSalesDetails>();
        public ICollection<DtoSalesService> ChildDtoSalesServices { get; set; } = new List<DtoSalesService>(); 
        #endregion
    }
}

