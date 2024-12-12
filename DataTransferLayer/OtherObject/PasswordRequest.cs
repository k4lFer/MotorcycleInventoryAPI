using System.ComponentModel.DataAnnotations;

namespace DataTransferLayer.OtherObject
{
    public class PasswordRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener minimo 6 caracteres.")]
        public string password { get; set; }
    }
}
