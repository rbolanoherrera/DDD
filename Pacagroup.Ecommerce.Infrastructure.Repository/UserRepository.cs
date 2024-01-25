using Dapper;
using Pacagroup.Ecommerce.Domain.Entity;
using Pacagroup.Ecommerce.Infrastructure.Interface;
using Pacagroup.Ecommerce.Transversal.Common;

namespace Pacagroup.Ecommerce.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IConnectionFactory connectionFactory;

        public UserRepository(IConnectionFactory connectionFactory)
        {
            this.connectionFactory = connectionFactory;
        }

        public User Authenticate(string username, string password)
        {
            using(var connection = connectionFactory.GetConnection)
            {
                string query = "UsersGetByUserAndPassword";
                var parameters = new DynamicParameters();
                parameters.Add("username", username);
                parameters.Add("password", password);

                User user = connection.QuerySingle<User>(query, param: parameters, commandType: System.Data.CommandType.StoredProcedure);
                return user;
            }
        }
    }
}