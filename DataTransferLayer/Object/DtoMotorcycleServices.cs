namespace DataTransferLayer.Object
{
    public class DtoMotorcycleServices
    {
        public Guid? id { get; set; }
        public Guid? saleId { get; set; }
        public Guid? motorcycleInstanceId { get; set; }
        public string? motorcycleName { get; set; }
        public Guid serviceId { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
        public decimal subtotal { get; set; }

        #region Parents

        public DtoService? ParentDtoService { get; set; }       
        public DtoSales ParentDtoSales { get; set; } 
        public DtoMotorcycle? ParentDtoMotocycle { get; set; }
        #endregion
    }
}