using BusinessLayer.Generic;
using DataTransferLayer.Object;
using DataTransferLayer.OtherObject;

namespace BusinessLayer.Business.Authentication
{
    public partial class BusinessAuthentication : BusinessGeneric
    {
        public (DtoMessage, DtoAuthentication) SearchByUser(string username)
        {
            DtoAuthentication dtoAuth = new();
                DtoOwner? dtoOwner = qAuthentication.SearchByUsernameOwner(username);
                if (dtoOwner != null)
                {
                    dtoAuth.id = dtoOwner.id;
                    dtoAuth.username = dtoOwner.username;
                    dtoAuth.password = dtoOwner.password;
                    dtoAuth.role = dtoOwner.role;
                    dtoAuth.status = dtoOwner.status;
                    _message.Success();
                }
            return (_message, dtoAuth);
        }

        public bool Authenticate(string dtoPassword, string password)
        {
            if (BCrypt.Net.BCrypt.Verify(password, dtoPassword))
            {
                return true;
            }
            return false;
        }
    }
}
