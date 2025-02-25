using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccessLayer.Generic;

namespace DataAccessLayer.Entity
{
    public class ServiceCategories : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        #region Childs
        public ICollection<Service> ChildServices { get; set; } = new List<Service>();
        #endregion
    }

}