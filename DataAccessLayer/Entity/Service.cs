using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Generic;
using DataTransferLayer.OtherObject;

namespace DataAccessLayer.Entity
{
    public class Service : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public Guid categoryId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public decimal price { get; set; }
        public StatusEnum status { get; set; }

        #region Parent
        public ServiceCategories? ParentServiceCategories { get; set; }
        #endregion

        #region Childs
        public ICollection<MotorcycleServices> ChildMotorcycleServices { get; set; } = new List<MotorcycleServices>();
        #endregion
    }
}
