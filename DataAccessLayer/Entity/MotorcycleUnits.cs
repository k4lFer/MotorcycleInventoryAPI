using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccessLayer.Generic;
using DataTransferLayer.OtherObject;

namespace DataAccessLayer.Entity
{
    public class MotorcycleUnits : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public Guid motorcycleId { get; set; }
        public string vin { get; set; }
        public StatusUnitEnum status { get; set; }
        public DateTime purchaseDate { get; set; }
        public decimal purchasePrice { get; set; }
        public decimal salePrice { get; set; }

        #region Parent
        public Motorcycle ParentMotorcycles { get; set; } = null!;
        #endregion
    }
}