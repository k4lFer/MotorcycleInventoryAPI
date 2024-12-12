using System.ComponentModel.DataAnnotations;
using DataTransferLayer.Generic;
using DataTransferLayer.OtherObject;

namespace DataTransferLayer.Object
{
    public class DtoOwner : DtoDateGeneric
    {
        public Guid id { get; set; }
        public string profilePictureUrl { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string dni { get; set; }
        public string? ruc { get; set; }
        public string address { get; set; }
        public string? phoneNumber { get; set; }
        [EnumDataType(typeof(Hierarchy), ErrorMessage = "El rol proporcionado no es válido.")]
        public Hierarchy role { get; set; }
        public bool status { get; set; }
        public Guid? createdBy { get; set; }
        public Guid? updatedBy { get; set; }
    }
}
