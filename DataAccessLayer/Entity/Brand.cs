using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using DataAccessLayer.Generic;

namespace DataAccessLayer.Entity
{
    public class Brand : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        //public string ruc {  get; set; }
        public string name { get; set; }
        //public string description { get; set; }

        #region Childs
        public ICollection<Motorcycle> ChildMotorcycle { get; set; } = new List<Motorcycle>();
        #endregion
    }
}
