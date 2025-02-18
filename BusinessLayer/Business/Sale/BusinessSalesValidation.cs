using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;

namespace BusinessLayer.Business.Sale
{
    public partial class BusinessSales
    {
        private void CreateSaleValidation(DtoSales dtoSales)
        {
            // Validar que al menos uno de los detalles o servicios esté presente
            if ((dtoSales.ChildDtoSalesMotorcycles == null || !dtoSales.ChildDtoSalesMotorcycles.Any()) &&
                (dtoSales.ChildDtoMotorcycleServices == null || !dtoSales.ChildDtoMotorcycleServices.Any()))
                {
                    _message.AddMessage("Debe incluir al menos un detalle de venta o un servicio.");

                }

            if (dtoSales.ChildDtoSalesMotorcycles != null)
            {
                foreach (var detail in dtoSales.ChildDtoSalesMotorcycles)
                {
                    var dtoMotorcycle = qmotorcycle.getById(detail.motorcycleId);
                    if (dtoMotorcycle == null)
                    {
                        _message.AddMessage($"La motocicleta no existe.");
                    }
                    else if (dtoMotorcycle.status == StatusEnum.not_available)
                    {
                        _message.AddMessage("La motocicleta no se encuentra disponible.");
                    }
                    else if (detail.quantity <= 0)
                    {
                        _message.AddMessage($"La cantidad para la motocicleta debe ser mayor a 0.");
                    }
                }
            }

            if (dtoSales.ChildDtoMotorcycleServices != null)
            {
                foreach (var service in dtoSales.ChildDtoMotorcycleServices)
                {
                    var dtoService = qService.getById(service.serviceId);
                    if (dtoService == null)
                    {
                        _message.AddMessage($"El servicio  no existe.");
                    }
                    else if (service.quantity <= 0)
                    {
                        _message.AddMessage($"La cantidad para el servicio debe ser mayor a 0.");
                    }
                }
            }
        }

    }

}

