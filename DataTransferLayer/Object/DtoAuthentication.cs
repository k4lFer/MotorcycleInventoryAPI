using System.ComponentModel.DataAnnotations;
using DataTransferLayer.OtherObject;

namespace DataTransferLayer.Object
{
    public class DtoAuthentication
    {
        public Guid id { get; set; }
        [Required]
        [StringLength(25, MinimumLength = 4, ErrorMessage = "El nombre de usuario debe tener entre 4 y 25 caracteres.")]
        public string username { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "La contraseña debe tener minimo 6 caracteres.")]
        public string password { get; set; }
        public Hierarchy role { get; set; }
        public bool status { get; set; }
    }
}
