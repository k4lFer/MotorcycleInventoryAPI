using DataAccessLayer.Generic;
using DataTransferLayer.OtherObject;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entity
{
    public class Motorcycle : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public Guid typeId { get; set; }
        public Guid brandId { get; set; }
        public string name { get; set; }
        public string displacement { get; set; }
        //public string vin { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public StatusEnum status { get; set; }

        #region Parent
        public Types ParentTypes { get; set; } = null!;
        public Brand ParentBrands { get; set; } = null!;
        #endregion

        #region Childs
        public ICollection<SalesMotorcycles> ChildSalesMotorcycles { get; set; } = new List<SalesMotorcycles>();
        public ICollection<MotorcycleServices>? ChildMotorcycleServices { get; set; } = new List<MotorcycleServices>();
        public ICollection<MotorcycleUnits>? ChildMotorcycleUnits { get; set; } = new List<MotorcycleUnits>(); 
        #endregion
    }
}
