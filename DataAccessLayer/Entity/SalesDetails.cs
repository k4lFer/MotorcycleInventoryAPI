using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entity
{
    public class SalesDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public Guid? saleId { get; set; }
        public Guid motorcycleId { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
        public decimal subTotal { get; set; }

        #region Parent
        public Sales ParentSales { get; set; } = null!;
        public Motorcycle ParentMotorcyle { get; set; } = null!;
        #endregion

    }
}
