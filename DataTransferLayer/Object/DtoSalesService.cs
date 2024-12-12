namespace DataTransferLayer.Object
{
    public class DtoSalesService
    {
        public Guid? id { get; set; }
        public Guid? saleId { get; set; }
        public Guid serviceId { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
        public decimal subtotal { get; set; }

        #region Parents

        public DtoService? ParentDtoService { get; set; }       
        public DtoSales ParentDtoSales { get; set; } 
        #endregion
    }
}