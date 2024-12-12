using System.Text.Json;
using System.Transactions;
using BusinessLayer.ExternalApi;
using BusinessLayer.Generic;
using CloudinaryDotNet;
using DataTransferLayer.Object;
using DataTransferLayer.Object.ResponseAPIsNet;
using DataTransferLayer.OtherObject;
using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Business.Owner
{
    public partial class BusinessOwner : BusinessGeneric
    {
        public async Task<DtoMessage> RegisterOwner(DtoOwner dtoOwner, Guid ownerId)
        {
            using TransactionScope transactionScope = new();
            InsertValidation(dtoOwner);
            if (_message.ExistsMessage())
            {
                _message.Error();
                return _message;
            }
            else
            {
                string passwordPlain = dtoOwner.password;
                dtoOwner.id = Guid.NewGuid();
                dtoOwner.password = BCrypt.Net.BCrypt.HashPassword(dtoOwner.password);
                dtoOwner.email = DataEncrypt.Encrypt(dtoOwner.email);
                dtoOwner.dni = DataEncrypt.Encrypt(dtoOwner.dni);
                dtoOwner.ruc = !string.IsNullOrEmpty(dtoOwner.ruc) ? DataEncrypt.Encrypt(dtoOwner.ruc) : null;
                dtoOwner.phoneNumber = !string.IsNullOrEmpty(dtoOwner.phoneNumber) ? DataEncrypt.Encrypt(dtoOwner.phoneNumber) : null;
                dtoOwner.status = true;
                dtoOwner.createdAt = DateTime.UtcNow;
                dtoOwner.updatedAt = DateTime.UtcNow;
                dtoOwner.createdBy = ownerId;
                dtoOwner.updatedBy = ownerId;
                dtoOwner.profilePictureUrl = CloudinaryService.GetDefaultProfilePicture();
                
                string emailPlain = DataEncrypt.Decrypt(dtoOwner.email);
                //var emailSent = await ResendApi.SendCredentialsAsync(emailPlain, dtoOwner.username, passwordPlain);
                /*
                 if (!emailSent)
                {
                    _message.AddMessage("Error al enviar las credenciales.");
                    _message.Error();
                    return _message; 
                }
                */
                await qOwner.Register(dtoOwner);
                transactionScope.Complete();
                _message.AddMessage("Operación realizada correctamente.");
                _message.Success();
                return _message;
                
            }
        }
        
        public DtoMessage UpdateOwner(DtoOwner dto, Guid ownerId)
        {
            using TransactionScope transactionScope = new();
            UpdateValidation(dto, ownerId);
            if (_message.ExistsMessage()) 
            {
                _message.Error();
                return _message;
            }
            
            DtoOwner? dtoOwner = qOwner.GetById(ownerId);
            if (dtoOwner != null)
            {
                dtoOwner.username = dto.username;
                dtoOwner.firstName = dto.firstName;
                dtoOwner.lastName = dto.lastName;
                dtoOwner.email = DataEncrypt.Encrypt(dto.email);
                dtoOwner.dni = DataEncrypt.Encrypt(dto.dni);
                dtoOwner.ruc = !string.IsNullOrEmpty(dto.ruc) ? DataEncrypt.Encrypt(dto.ruc) : null;
                dtoOwner.address = DataEncrypt.Encrypt(dto.address);
                dtoOwner.phoneNumber = !string.IsNullOrEmpty(dto.phoneNumber) ? DataEncrypt.Encrypt(dto.phoneNumber) : null;
                dtoOwner.updatedAt = DateTime.UtcNow;
                
                qOwner.Update(dtoOwner);
                transactionScope.Complete();
                _message.AddMessage("Operación realizada correctamente.");
                _message.Success();
            }
            else
            {
                _message.AddMessage("Usuario no encontrado.");
                _message.Error();
            }
            return _message;
        }

        public DtoMessage UpdateProfilePicture(Guid ownerId, IFormFile file)
        {
            using TransactionScope transactionScope = new();
            UploadProfilePicture(file);
            if (_message.ExistsMessage())
            {
                _message.Error();
                return _message;
            }
            var uploadResult = CloudinaryService.UploadProfilePicture(ownerId, file);
            var dtoOwner = qOwner.GetById(ownerId);

            if (uploadResult != null)
            {
                dtoOwner.profilePictureUrl = uploadResult;
                qOwner.Update(dtoOwner);
                transactionScope.Complete();
                _message.AddMessage("Imagen de perfil actualizada correctamente.");
                _message.Success();
            }
            
            else
            {
                _message.AddMessage("Error en la subida de imagen.");
                _message.Error();
            }
            
            return _message;
        }

        public DtoMessage DeleteProfilePicture(Guid id)
        {
            using TransactionScope transactionScope = new();
            DtoOwner? owner = qOwner.GetById(id);

            if (string.Equals(owner.profilePictureUrl, CloudinaryService.GetDefaultProfilePicture(), StringComparison.OrdinalIgnoreCase))
            {
                _message.AddMessage("No puede eliminar la imagen de perfil predeterminada.");
                _message.Conflict();
                return _message;
            }

            string publicId = CloudinaryService.ExtractPublicId(owner.profilePictureUrl);

            if (CloudinaryService.DeletePicture(publicId))
            {
                owner.profilePictureUrl = CloudinaryService.GetDefaultProfilePicture();
                qOwner.Update(owner);

                transactionScope.Complete(); 
                _message.AddMessage("Imagen de perfil eliminada exitosamente.");
                _message.Success();
                return _message;
            }
            _message.AddMessage("No se pudo eliminar la imagen de perfil.");
            _message.Error();
            return _message;
        }
        
        public DtoMessage ChangePassword(DtoPassword dto, Guid id)
        {
            DtoOwner? dtoOwner = qOwner.GetById(id);
            if (dtoOwner == null)
            {
                _message.AddMessage("Usuario no encontrado.");
                _message.Error();
                return _message;
            }

            if (BCrypt.Net.BCrypt.Verify(dto.oldpassword, dtoOwner.password))
            {
                dtoOwner.password = BCrypt.Net.BCrypt.HashPassword(dto.newpassword);
                dtoOwner.updatedAt = DateTime.UtcNow;
                qOwner.UpdatePassword(dtoOwner);
                _message.AddMessage("Contraseña cambiada exitosamente");
                _message.Success();
            }
            else
            {
                _message.AddMessage("La contraseña antigua no coinciden");
                _message.Error();
            }
            return _message;
        }

        public (DtoMessage, DtoOwner) MyProfile(Guid id)
        {
            DtoOwner dtoOwner = qOwner.MyProfile(id);
            dtoOwner.email = DataEncrypt.Decrypt(dtoOwner.email);
            dtoOwner.dni = DataEncrypt.Decrypt(dtoOwner.dni);
            dtoOwner.ruc = !string.IsNullOrEmpty(dtoOwner.ruc) ? DataEncrypt.Decrypt(dtoOwner.ruc) : null;
            dtoOwner.phoneNumber = !string.IsNullOrEmpty(dtoOwner.phoneNumber) ? DataEncrypt.Decrypt(dtoOwner.phoneNumber) : null;
            dtoOwner.password = null;
            _message.Success();
            return (_message, dtoOwner);
        }
        
        public DtoMessage DeleteUser(PasswordRequest dto, Guid id)
        {
            DtoOwner? dtoOwner = qOwner.GetById(id);
            if (dtoOwner == null)
            {
                _message.AddMessage("Usuario no encontrado.");
                _message.Error();
                return _message;
            }
            if (BCrypt.Net.BCrypt.Verify(dto.password, dtoOwner.password))
            {
                if (qOwner.DeleteOnCascadeOwner(id) != 0)
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
        
        public (DtoMessage, List<DtoOwner>) GetAll(Guid idOwner)
        {
            List<DtoOwner> listOwner = qOwner.GetAll(idOwner);
            if (listOwner.Count > 0)
            {
                foreach (DtoOwner dtoOwner in listOwner)
                {
                    dtoOwner.password = null;
                    dtoOwner.email = DataEncrypt.Decrypt(dtoOwner.email);
                    dtoOwner.dni = DataEncrypt.Decrypt(dtoOwner.dni);
                    dtoOwner.ruc = !string.IsNullOrEmpty(dtoOwner.ruc) ? DataEncrypt.Decrypt(dtoOwner.ruc) : null;
                    dtoOwner.phoneNumber = !string.IsNullOrEmpty(dtoOwner.phoneNumber) ? DataEncrypt.Decrypt(dtoOwner.phoneNumber) : null;
                }
                _message.Success();
                return (_message, listOwner);
            }
            _message.Warning();
            return (_message, listOwner);
        }
        
        public DtoMessage PromoteOrDemoteOwner(DtoOwner dto, Guid idOwner)
        {
            using TransactionScope transactionScope = new();
            ValidationPromoteOrDemote(dto);
            if (_message.ExistsMessage()) 
            {
                _message.Error();
                return _message;
            }
            
            DtoOwner? dtoOwner = qOwner.GetById(dto.id);
            if (dtoOwner != null)
            {
                dtoOwner.role = dto.role;
                dtoOwner.status = dto.status;
                dtoOwner.updatedAt = DateTime.UtcNow;
                dtoOwner.updatedBy = idOwner;
                qOwner.UpdatePromoteOrDemote(dtoOwner);
                transactionScope.Complete();
                _message.AddMessage("Operación realizada correctamente");
                _message.Success();
            }
            else
            {
                _message.AddMessage("Usuario no encontrado");
                _message.Error();
            }
            return _message;
        }
    }
}
