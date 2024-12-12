using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Generic;

namespace DataAccessLayer.Entity
{
    public class Service : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }

        #region Childs
        public ICollection<SalesService> ChildSalesServices { get; set; } = new List<SalesService>();
        #endregion
    }
}
