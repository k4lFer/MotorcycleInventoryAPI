using DataAccessLayer.Query;

namespace BusinessLayer.Business.Authentication
{
    public partial class BusinessAuthentication
    {
        private QAuthentication qAuthentication = new();
        private QUser qUser = new();
    }
}
