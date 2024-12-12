using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DataAccessLayer.Generic;
using DataTransferLayer.OtherObject;

namespace DataAccessLayer.Entity
{
    public class Owner : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public string? profilePictureUrl { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string dni { get; set; }
        public string? ruc { get; set; }
        public string address { get; set; }
        public string? phoneNumber { get; set; }
        public Hierarchy role { get; set; }
        public bool status { get; set; }
        public Guid? createdBy { get; set; }
        public Guid? updatedBy { get; set; }

        #region Childs
        public ICollection<Sales> ChildSales { get; set; } = new List<Sales>();
        #endregion
    }
}
