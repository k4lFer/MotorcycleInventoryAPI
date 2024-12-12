using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccessLayer.Generic;
using DataTransferLayer.OtherObject;

namespace DataAccessLayer.Entity
{
    public class User : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string documentType { get; set; }
        public string documentNumber { get; set; }
        public string? phoneNumber { get; set; }
        //public Hierarchy role { get; set; }
        //public bool status { get; set; }
        //public string? profilePictureUrl { get; set; }
        
        #region Childs
        public ICollection<Sales> ChildSales { get; set; } = new List<Sales>();
 
        #endregion
    }
}
