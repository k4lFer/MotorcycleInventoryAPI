using System.Transactions;
using BusinessLayer.Generic;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;

namespace BusinessLayer.Business.Sale
{
    public partial class BusinessSales : BusinessGeneric
    {
        public (DtoMessage, List<DtoSales>) GetAllSales()
        {
            List<DtoSales> listSales = qSales.getAll();
            if (listSales.Count > 0)
            {
                foreach (DtoSales dtoSales in listSales)
                {
                    dtoSales.ownerId = null;
                    dtoSales.userId = null;
                }
                _message.Success();
                return (_message, listSales);
            }
            _message.Warning();
            return (_message, listSales);
        }

        public (DtoMessage, DtoSales) GetSaleById(Guid saleId)
        {
            DtoSales? dtoSales = qSales.getById(saleId);
            if(dtoSales != null)
            {
                _message.Success();
                return (_message, dtoSales);
            }
            _message.Error();
            return (_message, dtoSales);
        }

        public (DtoMessage, DtoSales) GetSaleByOwnerId(Guid id)
        {
            DtoSales? dtoSales = qSales.getByOwnerId(id);
            if(dtoSales != null)
            {
                _message.Success();
                return (_message, dtoSales);
            }
            _message.Error();
            return (_message, dtoSales);
        }
        
        public DtoMessage CreateSale(DtoSales dtoSales)
        {
            using (TransactionScope transactionScope = new(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
            {
                CreateSaleValidation(dtoSales);
                if (_message.ExistsMessage())
                {
                    _message.Conflict();
                    return _message;
                }

                dtoSales.id = Guid.NewGuid();
                dtoSales.date = DateTime.UtcNow;
                dtoSales.totalPrice = 0;
                dtoSales.quantity = 0;
                
                qSales.create(dtoSales);

                if (dtoSales.ChildDtoSalesDetails?.Any() == true)
                {
                    foreach (var dtoSalesDetails in dtoSales.ChildDtoSalesDetails)
                    {
                        ProcessSaleDetail(dtoSales, dtoSalesDetails);
                    }
                }

                if (dtoSales.ChildDtoSalesServices?.Any() == true)
                {
                    foreach (var dtoSalesService in dtoSales.ChildDtoSalesServices)
                    {
                        ProcessSaleService(dtoSales, dtoSalesService);
                    }
                }
                
                qSales.update(dtoSales);

                transactionScope.Complete();

                _message.AddMessage("Venta creada exitosamente.");
                _message.Success();
                return _message;
            }
        }

        private void ProcessSaleDetail(DtoSales dtoSales, DtoSalesDetails dtoSalesDetails)
        {
            var dtoMotorcycle = qmotorcycle.getById(dtoSalesDetails.motorcycleId);
            
            dtoSalesDetails.id = Guid.NewGuid();
            dtoSalesDetails.unitPrice = dtoMotorcycle.price;
            dtoSalesDetails.saleId = dtoSales.id;
            dtoSalesDetails.subTotal = dtoSalesDetails.quantity * dtoSalesDetails.unitPrice;
            
            dtoMotorcycle.quantity -= dtoSalesDetails.quantity;
            if (dtoMotorcycle.quantity == 0)
            {
                dtoMotorcycle.status = StatusEnum.not_available;
            }
            
            qmotorcycle.update(dtoMotorcycle);
            
            dtoSales.totalPrice += dtoSalesDetails.subTotal;
            dtoSales.quantity += dtoSalesDetails.quantity;
            
            qSalesDetails.create(dtoSalesDetails);
        }

        private void ProcessSaleService(DtoSales dtoSales, DtoSalesService dtoSalesService)
        {
            var dtoService = qService.getById(dtoSalesService.serviceId);

            dtoSalesService.id = Guid.NewGuid();
            dtoSalesService.unitPrice = dtoService.price;
            dtoSalesService.saleId = dtoSales.id;
            dtoSalesService.subtotal = dtoSalesService.unitPrice * dtoSalesService.quantity;
            
            dtoSales.totalPrice += dtoSalesService.subtotal;
            dtoSales.quantity += dtoSalesService.quantity;
            
            qSalesService.create(dtoSalesService);
        }
        
    }
}

