using System.Transactions;
using BusinessLayer.ExternalApi;
using BusinessLayer.Generic;
using DataAccessLayer.Query;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Business.User
{
    public partial class BusinessUser : BusinessGeneric
    {
        public DtoMessage RegisterUser(DtoUser dtoUser)
        {
            using TransactionScope transactionScope = new();
            InsertValidation(dtoUser);
            if (_message.ExistsMessage())
            {
                _message.Error();
                return _message;
            }
            dtoUser.id = Guid.NewGuid();
            dtoUser.email = DataEncrypt.Encrypt(dtoUser.email);
            dtoUser.phoneNumber = !string.IsNullOrEmpty(dtoUser.phoneNumber) ? DataEncrypt.Encrypt(dtoUser.phoneNumber) : null;
            //dtoUser.profilePictureUrl = CloudinaryService.GetDefaultProfilePicture();
            //dtoUser.role = Hierarchy.Logged;
           // dtoUser.status = true;
            dtoUser.createdAt = DateTime.UtcNow;
            dtoUser.updatedAt = DateTime.UtcNow;

            qUser.Register(dtoUser);
            transactionScope.Complete();
            _message.AddMessage("Operación realizada correctamente.");
            _message.Success();
            return _message;
        }
        
        public DtoMessage UpdateUser(DtoUser dto, Guid id)
        {
            using TransactionScope transactionScope = new();
            UpdateValidation(dto, id);
            if (_message.ExistsMessage()) 
            {
                _message.Error();
                return _message;
            }
            
            DtoUser? dtoUser = qUser.GetById(id);
            if (dtoUser != null)
            {
                dtoUser.firstName = dto.firstName;
                dtoUser.lastName = dto.lastName;
                dtoUser.email = DataEncrypt.Encrypt(dto.email);
                dtoUser.phoneNumber = !string.IsNullOrEmpty(dto.phoneNumber) ? DataEncrypt.Encrypt(dto.phoneNumber) : null;
                dtoUser.updatedAt = DateTime.UtcNow;
                
                qUser.Update(dtoUser);
                transactionScope.Complete();
                _message.AddMessage("Operación realizada correctamente.");
                _message.Success();
                return _message;
            }
            _message.AddMessage("Usuario no encontrado.");
            _message.Error();
            return _message;
        }
        
        /*public DtoMessage updateProfilePictureUser(Guid userId, IFormFile file)
        {
            using TransactionScope transactionScope = new();
            UploadProfilePicture(file);
            if (_message.ExistsMessage())
            {
                _message.Error();
                return _message;
            }
            var uploadResult = CloudinaryService.UploadProfilePicture(userId, file);
            var dtoUser = qUser.GetById(userId);

            if(uploadResult != null)
            {
                dtoUser.profilePictureUrl = uploadResult;
                qUser.Update(dtoUser);
                transactionScope.Complete();
                _message.AddMessage("Imagen de perfil actualizada correctamente");
                _message.Success();
            }
            
            else{
                _message.AddMessage("Error en la subida de la imagen");
                _message.Error();
            }
            
            return _message;
        }

*/
        public (DtoMessage, DtoUser) getById(Guid id)
        {
            DtoUser dtoUser = qUser.GetById(id);
            dtoUser.email = DataEncrypt.Decrypt(dtoUser.email);
            dtoUser.phoneNumber = !string.IsNullOrEmpty(dtoUser.phoneNumber) ? DataEncrypt.Decrypt(dtoUser.phoneNumber) : null;
            _message.Success();
            return (_message, dtoUser);
        }
        
        public (DtoMessage, DtoUser) getByDocumentNumber(string documentNumber)
        {
            DtoUser dtoUser = qUser.GetByDocumentNumber(documentNumber);
            dtoUser.email = DataEncrypt.Decrypt(dtoUser.email);
            dtoUser.phoneNumber = !string.IsNullOrEmpty(dtoUser.phoneNumber) ? DataEncrypt.Decrypt(dtoUser.phoneNumber) : null;
            _message.Success();
            return (_message, dtoUser);
        }
       /* public DtoMessage DeleteProfilePicture(Guid userId){
            using TransactionScope transactionScope = new();
            DtoUser? dtoUser = qUser.GetById(userId);
            
            if (string.Equals(dtoUser.profilePictureUrl, CloudinaryService.GetDefaultProfilePicture(), StringComparison.OrdinalIgnoreCase))
            {
                _message.AddMessage("No puede eliminar la imagen de perfil predeterminada.");
                _message.Conflict();
                return _message;
            }

            string publicId = CloudinaryService.ExtractPublicId(dtoUser.profilePictureUrl);
            if(CloudinaryService.DeletePicture(publicId)){
                dtoUser.profilePictureUrl = CloudinaryService.GetDefaultProfilePicture();
                qUser.Update(dtoUser);
                transactionScope.Complete();

                _message.AddMessage("Imagen de perfil eliminada exitosamente.");
                _message.Success();
                return _message;
            }
            _message.AddMessage("Error al eliminar la imagen de perfil.");
            _message.Error();
            return _message;
        }
*/
        public DtoMessage DeleteUser(PasswordRequest dto, Guid userId, Guid ownerId)
        {
            DtoOwner? dtoOwner = qOwner.GetById(ownerId);
            DtoUser? dtoUser = qUser.GetById(userId);
            if (dtoUser == null)
            {
                _message.AddMessage("Usuario no encontrado.");
                _message.Error();
                return _message;
            }
            if (BCrypt.Net.BCrypt.Verify(dto.password, dtoOwner.password))
            {
                if (qUser.DeleteOnCascade(userId) != 0)
                {
                    _message.AddMessage("Usuario eliminado correctamente.");
                    _message.Success();
                }
            }
            else
            {
                _message.AddMessage("Contraseña incorrecta, intentalo nuevamente.");
                _message.Error();
            }
            return _message;
        }
        
        public (DtoMessage, List<DtoUser>) GetAll()
        {
            List<DtoUser> listUser = qUser.GetAll();
            if (listUser.Count > 0)
            {
                foreach (DtoUser dtoUser in listUser)
                {
                    dtoUser.email = DataEncrypt.Decrypt(dtoUser.email);
                    dtoUser.phoneNumber = !string.IsNullOrEmpty(dtoUser.phoneNumber) ? DataEncrypt.Decrypt(dtoUser.phoneNumber) : null;
                }
                _message.Success();
                return (_message, listUser);
            }
            _message.Warning();
            return (_message, listUser);
        }
        
    }
}
