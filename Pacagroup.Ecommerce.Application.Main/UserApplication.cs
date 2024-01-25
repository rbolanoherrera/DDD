using Pacagroup.Ecommerce.Application.DTO;
using Pacagroup.Ecommerce.Application.Interface;
using Pacagroup.Ecommerce.Domain.Entity;
using Pacagroup.Ecommerce.Domain.Interface;
using Pacagroup.Ecommerce.Transversal.Common;
using Pacagroup.Ecommerce.Transversal.Mapper;

namespace Pacagroup.Ecommerce.Application.Main
{
    public class UserApplication : IUserApplication
    {
        private readonly IUserDomain userDomain;
        private readonly UserBuilder userBuilder;

        public UserApplication(IUserDomain userDomain, UserBuilder userBuilder)
        {
            this.userDomain = userDomain;
            this.userBuilder = userBuilder;
        }

        public Response<UserDTO> Authenticate(string username, string password)
        {
            var response = new Response<UserDTO>();

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                response.Message = "Los parametros son requeridos";
                
                return response;
            }

            try
            {
                User user = userDomain.Authenticate(username, password);

                if (user != null)
                {
                    response.IsSuccess = true;
                    response.Data = userBuilder.Convert(user);
                    response.Message = "Autenticación de Usuario exitosa";
                }
                else
                {
                    response.IsSuccess = false;
                    response.Data = null;
                    response.Message = "Usuario o contraseña Invalidos";
                }
            }
            catch (InvalidOperationException)
            {
                response.IsSuccess = true;
                response.Message = "Usuario no existe";
            }
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}
