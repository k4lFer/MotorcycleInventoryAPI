using BusinessLayer.Config;
using BusinessLayer.ExternalApi;
using CloudinaryDotNet;
using DataAccessLayer.Query;

namespace BusinessLayer.Business.User
{
    public partial class BusinessUser
    {
        private QUser qUser = new();
        private QOwner qOwner = new();
        private QAuthentication qAuthentication = new();
        private EncryptWithAes DataEncrypt = new();
    }
}