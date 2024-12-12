using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DataAccessLayer.Entity
{
    public class Sales
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public Guid userId { get; set; }
        public Guid ownerId {  get; set; }
        public int quantity { get; set; }
        public decimal totalPrice { get; set; }
        public DateTime date { get; set; }

        #region Parent
        public User ParentUser { get; set; } = null!;
        public Owner ParentOwner { get; set; } = null!;
        #endregion

        #region Childs
        public ICollection<SalesDetails> ChildSalesDetails { get; set; } = new List<SalesDetails>();
        public ICollection<SalesService> ChildSalesServices { get; set; } = new List<SalesService>();
        #endregion

    }
}
