namespace DataTransferLayer.Object
{
    public class DtoSalesDetails
    {
        public Guid? id { get; set; }
        public Guid? saleId { get; set; }
        public Guid motorcycleId { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
        public decimal subTotal { get; set; }

        #region Parents
        public DtoSales? ParentDtoSales { get; set; }
        public DtoMotorcycle? ParentDtoMotorcycle { get; set; } 
        #endregion
    
    }
}

