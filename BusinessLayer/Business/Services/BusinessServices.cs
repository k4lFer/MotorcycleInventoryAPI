using System.Transactions;
using BusinessLayer.Generic;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;

namespace BusinessLayer.Business.Services{
    public partial class BusinessServices : BusinessGeneric
    {
        public DtoMessage registerService(DtoService dto)
        {
            using TransactionScope transactionScope = new();
            dto.id = Guid.NewGuid();
            dto.createdAt = DateTime.UtcNow;
            dto.updatedAt = DateTime.UtcNow;
            qService.create(dto);
            transactionScope.Complete();
            _message.AddMessage("service created successfully");
            _message.Success();
            return _message;
        }

        public DtoMessage updateService(DtoService dto)
        {
            using TransactionScope transactionScope = new();
            DtoService? dtoService = qService.getById(dto.id);
            if(dtoService != null)
            {
                dtoService.name = dto.name;
                dtoService.description = dto.description;
                dtoService.updatedAt = DateTime.UtcNow;
                qService.update(dtoService);
                transactionScope.Complete();
                _message.AddMessage("service updated successfully");
                _message.Success();
                return _message;
            }
            _message.AddMessage("service not found");
            _message.Error();
            return _message;
        }

        public (DtoMessage, List<DtoService>) GetAllServices()
        {
            List<DtoService> listServices = qService.getAll();
            if(listServices.Count > 0)
            {
                _message.Success();
                return (_message, listServices);
            }
            _message.Warning();
            return (_message, listServices);
        }
        public (DtoMessage, DtoService) GetServiceById(Guid id)
        {
            DtoService? dtoService = qService.getById(id);
            _message.Success();
            return (_message, dtoService);
        }

    }

}

