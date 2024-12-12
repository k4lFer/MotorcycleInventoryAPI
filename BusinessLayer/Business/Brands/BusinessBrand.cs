using System.Transactions;
using BusinessLayer.Generic;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;

namespace BusinessLayer.Business.Brands
{
    public partial class BusinessBrand : BusinessGeneric
    {
        public DtoMessage registerBrand(DtoBrand dto)
        {
            using TransactionScope transactionScope = new();
            dto.id = Guid.NewGuid();
            dto.createdAt = DateTime.UtcNow;
            dto.updatedAt = DateTime.UtcNow;
            qbrand.create(dto);
            transactionScope.Complete();
            _message.AddMessage("Brand created successfully");
            _message.Success();
            return _message;
        }

        public (DtoMessage, List<DtoBrand>) GetAllBrands()
        {
            List<DtoBrand> listBrands = qbrand.getAll();
            if (listBrands.Count > 0)
            {
                foreach (DtoBrand dtoBrand in listBrands)
                {
                    
                }
                _message.Success();
                return (_message, listBrands);
            }
            _message.Warning();
            return (_message, listBrands);
        }

        public (DtoMessage, DtoBrand) GetBrandById(Guid id)
        {
            DtoBrand? dtoBrand = qbrand.getById(id);
            _message.Success();
            return (_message, dtoBrand);
        }

        public DtoMessage updateBrand(DtoBrand dto)
        {
            using TransactionScope transactionScope = new();
            DtoBrand? dtoBrand = qbrand.getById(dto.id);
            if (dtoBrand != null)
            {
                dtoBrand.name = dto.name;
                dtoBrand.ruc = dto.ruc;
                //dtoBrand.description = dto.description;
                dtoBrand.updatedAt = DateTime.UtcNow;
                qbrand.update(dtoBrand);
                transactionScope.Complete();
                _message.AddMessage("Brand updated successfully");
                _message.Success();
                return _message;
            }
            _message.AddMessage("Brand not found");
            _message.Error();
            return _message;
        }
    }
}

