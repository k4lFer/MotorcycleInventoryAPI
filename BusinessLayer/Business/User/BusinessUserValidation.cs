using DataTransferLayer.Object;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace BusinessLayer.Business.User
{
    public partial class BusinessUser
    {
        private void InsertValidation(DtoUser dto)
        {
           
            if (qAuthentication.ExistByEmailUser(DataEncrypt.Encrypt(dto.email)))
            {
                _message.AddMessage("El (correo) ya se encuentra registrado");
            }

            string? number = !string.IsNullOrEmpty(dto.phoneNumber) ? DataEncrypt.Encrypt(dto.phoneNumber) : null;
            if (number != null && qAuthentication.ExistByPhoneNumberUser(number))
            {
                _message.AddMessage("El (número de celular) ya se encuentra registrado");
            }
        }
            
        private void UpdateValidation(DtoUser dto, Guid id)
        {
            DtoUser? dtoUserEmailTemp = qAuthentication.GetByEmailUser(DataEncrypt.Encrypt(dto.email));
            if (dtoUserEmailTemp is not null && dtoUserEmailTemp.id != id)
            {
                _message.AddMessage("El (correo) ya se encuentra registrado, ingrese otro");
            }

            string? number = !string.IsNullOrEmpty(dto.phoneNumber) ? DataEncrypt.Encrypt(dto.phoneNumber) : null;
            if (number != null)
            {
                DtoUser? dtoUserPhoneTemp = qAuthentication.GetByPhoneNumberUser(number);
                if (dtoUserPhoneTemp is not null && dtoUserPhoneTemp.id != id)
                {
                    _message.AddMessage("El (número de celular) ya se encuentra registrado, ingrese otro");
                }
            }
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
