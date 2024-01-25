using Pacagroup.Ecommerce.Application.DTO;
using Pacagroup.Ecommerce.Transversal.Common;

namespace Pacagroup.Ecommerce.Application.Interface
{
    public interface IUserApplication
    {
        Response<UserDTO> Authenticate(string username, string password);
    }
}