using DataTransferLayer.Object;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace BusinessLayer.Business.Owner
{
    public partial class BusinessOwner
    {
        private void InsertValidation(DtoOwner dto)
        {
            if (qAuthentication.ExistByUsernameOwner(dto.username))
            {
                _message.AddMessage("Usuario no disponible (este usuario ya se encuentra registrado)");
            }
            
            if (qAuthentication.ExistByEmailOwner(DataEncrypt.Encrypt(dto.email)))
            {
                _message.AddMessage("El (correo) ya se encuentra registrado");
            }
            
            if (qAuthentication.ExistByDniOwner(DataEncrypt.Encrypt(dto.dni)))
            {
                _message.AddMessage("El (DNI) ya se encuentra registrado");
            }
            
            string? ruc = !string.IsNullOrEmpty(dto.ruc) ? DataEncrypt.Encrypt(dto.ruc) : null;
            if (qAuthentication.ExistByRucOwner(ruc))
            {
                _message.AddMessage("El (RUC) ya se encuentra registrado");
            }

            string? number = !string.IsNullOrEmpty(dto.phoneNumber) ? DataEncrypt.Encrypt(dto.phoneNumber) : null;
            if (number != null && qAuthentication.ExistByPhoneNumberOwner(number))
            {
                _message.AddMessage("El (número de celular) ya se encuentra registrado");
            }
        }
            
        private void UpdateValidation(DtoOwner dto, Guid id)
        {
            DtoOwner? dtoUserUsernameTemp = qAuthentication.GetByUsernameOwner(dto.username);
            if (dtoUserUsernameTemp is not null && dtoUserUsernameTemp.id != id)
            {
                _message.AddMessage("El (usuario) ya se encuentra registrado, ingrese otro");
            }
            
            DtoOwner? dtoUserEmailTemp = qAuthentication.GetByEmailOwner(DataEncrypt.Encrypt(dto.email));
            if (dtoUserEmailTemp is not null && dtoUserEmailTemp.id != id)
            {
                _message.AddMessage("El (correo) ya se encuentra registrado, ingrese otro");
            }

            DtoOwner? dtoUserDniTemp = qAuthentication.GetByDniOwner(DataEncrypt.Encrypt(dto.dni));
            if (dtoUserDniTemp is not null && dtoUserDniTemp.id != id)
            {
                _message.AddMessage("El (DNI) ya se encuentra registrado, ingrese otro");
            }
            
            string? ruc = !string.IsNullOrEmpty(dto.ruc) ? DataEncrypt.Encrypt(dto.ruc) : null;
            if (ruc != null)
            {
                DtoOwner? dtoUserRucTemp = qAuthentication.GetByRucOwner(ruc);
                if (dtoUserRucTemp is not null && dtoUserRucTemp.id != id)
                {
                    _message.AddMessage("El (RUC) ya se encuentra registrado, ingrese otro");
                }
            }
            
            string? number = !string.IsNullOrEmpty(dto.phoneNumber) ? DataEncrypt.Encrypt(dto.phoneNumber) : null;
            if (number != null)
            {
                DtoOwner? dtoUserPhoneTemp = qAuthentication.GetByPhoneNumberOwner(number);
                if (dtoUserPhoneTemp is not null && dtoUserPhoneTemp.id != id)
                {
                    _message.AddMessage("El (número de celular) ya se encuentra registrado, ingrese otro");
                }
            }
        }

        private void ValidationPromoteOrDemote(DtoOwner dto)
        {
            
        }

        private void UploadProfilePicture(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                _message.AddMessage("El formato de la imagen no es valido. " +
                                    "Por favor suba una imagen con formato (.jpg, .jpeg, .png)");
            }

            using var stream = file.OpenReadStream();
            var image = Image.Load<Rgba32>(stream);
            if (image.Width < 800 || image.Width > 1020 || image.Height < 800 || image.Height > 1020)
            {
                _message.AddMessage("Las dimensiones de la imagen deben estar entre 800x800 y 1020x1020 píxeles.");
            }
        }
    }
}