using Pacagroup.Ecommerce.Domain.Entity;
using Pacagroup.Ecommerce.Domain.Interface;
using Pacagroup.Ecommerce.Infrastructure.Interface;

namespace Pacagroup.Ecommerce.Domain.Core
{
    public class UserDomain : IUserDomain
    {
        private readonly IUserRepository userRepository;

        public UserDomain(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User Authenticate(string username, string password)
        {
            return userRepository.Authenticate(username, password);
        }
    }
}
