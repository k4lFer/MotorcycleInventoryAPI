using System.Transactions;
using BusinessLayer.Generic;
using BusinessLayer.Signals;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.SignalR;

namespace BusinessLayer.Business.Motorcycle
{
    public partial class BusinessMotorcycle : BusinessGeneric
    {
        //private readonly IHubContext<StockAlertHub> _hubContext;
        /*public BusinessMotorcycle(IHubContext<StockAlertHub> hubContext)
        {
            _hubContext = hubContext;
        }*/

        //public BusinessMotorcycle() { }

        public DtoMessage registerMotorcycle(DtoMotorcycle dto)
        {
            using TransactionScope transactionScope = new();
            dto.id = Guid.NewGuid();
            dto.status = StatusEnum.available;
            dto.createdAt = DateTime.UtcNow;
            dto.updatedAt = DateTime.UtcNow;
            qMotorcycle.create(dto);
            transactionScope.Complete();
            _message.AddMessage("motorcycle created successfully");
            _message.Success();
            return _message;
        }

        public DtoMessage updateMotocycle(DtoMotorcycle dto)
        {
            using TransactionScope transactionScope = new();
            DtoMotorcycle? dtoMotorcycle = qMotorcycle.getById(dto.id);
            if (dtoMotorcycle != null)
            {
                dtoMotorcycle.name = dto.name;
                dtoMotorcycle.brandId = dto.brandId;
                dtoMotorcycle.typeId = dto.typeId;
                dtoMotorcycle.quantity = dto.quantity;
                dtoMotorcycle.displacement = dto.displacement;
                dtoMotorcycle.price = dto.price;
                //dtoMotorcycle.status = dto.status;
                dtoMotorcycle.updatedAt = DateTime.UtcNow;

                if (dtoMotorcycle.quantity > 0 && dtoMotorcycle.status == StatusEnum.not_available)
                {
                    dtoMotorcycle.status = StatusEnum.available;
                }
                else if (dtoMotorcycle.quantity == 0)
                {
                    dtoMotorcycle.status = StatusEnum.not_available;
                }
                qMotorcycle.update(dtoMotorcycle);
                transactionScope.Complete();
                _message.AddMessage("motorcycle updated successfully");
                _message.Success();
                return _message;
            }
            _message.AddMessage("motorcycle not found");
            _message.Error();
            return _message;
        }

        public DtoMessage disableMotorcycle(Guid id)
        {
            using TransactionScope transactionScope = new();
            DtoMotorcycle? dtoMotorcycle = qMotorcycle.getById(id);
            if (dtoMotorcycle != null)
            {
                dtoMotorcycle.status = StatusEnum.not_available;
                qMotorcycle.update(dtoMotorcycle);
                transactionScope.Complete();
                _message.AddMessage("motorcycle disabled successfully");
                _message.Success();
                return _message;
            }
            _message.AddMessage("motorcycle not found");
            _message.Error();
            return _message;
        }

        public DtoMessage enableMotorcycle(Guid id)
        {
            using TransactionScope transactionScope = new();
            DtoMotorcycle? dtoMotorcycle = qMotorcycle.getById(id);
            if (dtoMotorcycle != null)
            {
                dtoMotorcycle.status = StatusEnum.available;
                qMotorcycle.update(dtoMotorcycle);
                transactionScope.Complete();
                _message.AddMessage("motorcycle enabled successfully");
                _message.Success();
                return _message;
            }
            _message.AddMessage("motorcycle not found");
            _message.Error();
            return _message;
        }

        public (DtoMessage, List<DtoMotorcycle>) GetAllMotorcycle()
        {
            List<DtoMotorcycle> listMotorcycles = qMotorcycle.getAll();
            if (listMotorcycles.Count > 0)
            {
                foreach (DtoMotorcycle dtoMotorcycle in listMotorcycles)
                {
                    dtoMotorcycle.brandId = null;
                    dtoMotorcycle.typeId = null;
                   // CheckAndEmitAlerts(dtoMotorcycle).Wait();
                }
                _message.Success();
                return (_message, listMotorcycles);
            }
            _message.Warning();
            return (_message, listMotorcycles);
        }

        public (DtoMessage, DtoMotorcycle) GetMotorcycleId(Guid id)
        {
            DtoMotorcycle? dtoMotorcycle = qMotorcycle.getById(id);
            dtoMotorcycle.brandId = null;
            dtoMotorcycle.typeId = null;
            //CheckAndEmitAlerts(dtoMotorcycle).Wait();
            _message.Success();
            return (_message, dtoMotorcycle);
        }
        
        /*private async Task CheckAndEmitAlerts(DtoMotorcycle motorcycle)
        {
            if (motorcycle.quantity == 0)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveAlert", $"ALERTA: La motocicleta '{motorcycle.name}' está sin stock.");
            }
            else if (motorcycle.quantity <= 2)
            {
                await _hubContext.Clients.All.SendAsync("ReceiveAlert", $"ALERTA: La motocicleta '{motorcycle.name}' tiene un stock crítico: {motorcycle.quantity} unidad(es).");
            }
        }*/

    }
}

