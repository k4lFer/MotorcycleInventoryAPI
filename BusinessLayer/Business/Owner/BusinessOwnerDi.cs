using BusinessLayer.Config;
using BusinessLayer.ExternalApi;
using CloudinaryDotNet;
using DataAccessLayer.Query;

namespace BusinessLayer.Business.Owner
{
    public partial class BusinessOwner
    {
        private QOwner qOwner = new();
        private QAuthentication qAuthentication = new();
        private EncryptWithAes DataEncrypt = new();
    }
}