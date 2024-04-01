using Pacagroup.Ecommerce.Infrastructure.Interface;

namespace Pacagroup.Ecommerce.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICustomerRepository Customers { get; }

        public IUserRepository Users { get; }


        public UnitOfWork(ICustomerRepository customerRepository, IUserRepository userRepository)
        {
            Customers = customerRepository;
            Users = userRepository;
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }
    }
}