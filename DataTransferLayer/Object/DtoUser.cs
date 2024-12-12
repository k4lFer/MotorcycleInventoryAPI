using System.ComponentModel.DataAnnotations;
using DataTransferLayer.Generic;
using DataTransferLayer.OtherObject;

namespace DataTransferLayer.Object
{
    public class DtoUser : DtoDateGeneric
    {
        public Guid id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string firstName { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El apellido debe tener entre 3 y 100 caracteres.")]
        public string lastName { get; set; }
        public string documentType { get; set; }
        public string documentNumber { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "El formato del correo electrónico es incorrecto.")]
        public string email { get; set; }
        public string? phoneNumber { get; set; }
        //public Hierarchy role { get; set; }
        //public bool status { get; set; }
        //public string profilePictureUrl { get; set; }
    }
}
