using System.Transactions;
using BusinessLayer.Generic;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;

namespace BusinessLayer.Business.Motorcycle
{
    public partial class BusinessMotorcycle : BusinessGeneric
    {
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
            _message.Success();
            return (_message, dtoMotorcycle);
        }
    }
}

