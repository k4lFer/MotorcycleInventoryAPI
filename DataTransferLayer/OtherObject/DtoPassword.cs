using System.ComponentModel.DataAnnotations;

namespace DataTransferLayer.OtherObject
{
    public class DtoPassword
    {
        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 20 caracteres.")]
        public string oldpassword { get; set; }
        [Required]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 20 caracteres.")]
        public string newpassword { get; set; }
    }
}
