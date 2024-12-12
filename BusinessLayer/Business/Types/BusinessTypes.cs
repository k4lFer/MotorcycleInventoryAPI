using System.Transactions;
using BusinessLayer.Generic;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;

namespace BusinessLayer.Business.Types
{
    public partial class BusinessTypes : BusinessGeneric
    {
        public DtoMessage registerType(DtoTypes dtoType)
        {
            using TransactionScope transactionScope = new();
            dtoType.id = Guid.NewGuid();
            dtoType.createdAt = DateTime.UtcNow;
            dtoType.updatedAt = DateTime.UtcNow;
            qTypes.create(dtoType);
            transactionScope.Complete();
            _message.AddMessage("Types registered successfully");
            _message.Success();
            return _message;
        }

        public (DtoMessage, List<DtoTypes>) GetAllTypes()
        {
            List<DtoTypes> listType = qTypes.getAll();
            if (listType.Count > 0)
            {
                _message.Success();
                return (_message, listType);
            }
            _message.Warning();
            return (_message, listType);
        }

        public (DtoMessage, DtoTypes) GetTypesById(Guid id)
        {
            DtoTypes? dtoTypes = qTypes.getById(id);
            _message.Success();
            return (_message, dtoTypes);
        }

        public DtoMessage updateType(DtoTypes dto)
        {
            using TransactionScope transactionScope = new();
            DtoTypes? dtoTypes = qTypes.getById(dto.id);
            if (dtoTypes != null)
            {
                dtoTypes.name = dto.name;
                dtoTypes.description = dto.description;
                dtoTypes.updatedAt = DateTime.UtcNow;
                qTypes.update(dtoTypes);
                transactionScope.Complete();
                _message.AddMessage("Types updated successfully");
                _message.Success();
                return _message;
            }
            _message.AddMessage("Types not found");
            _message.Warning();
            return _message;
        }
    }
}

