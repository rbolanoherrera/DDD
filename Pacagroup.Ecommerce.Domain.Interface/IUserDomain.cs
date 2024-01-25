using Pacagroup.Ecommerce.Domain.Entity;

namespace Pacagroup.Ecommerce.Domain.Interface
{
    public interface IUserDomain
    {
        User Authenticate(string username, string password);
    }
}