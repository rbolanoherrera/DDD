using Pacagroup.Ecommerce.Domain.Entity;
using Pacagroup.Ecommerce.Domain.Interface;
using Pacagroup.Ecommerce.Infrastructure.Interface;

namespace Pacagroup.Ecommerce.Domain.Core
{
    public class CustomerDomain : ICustomerDomain
    {
        private readonly ICustomerRepository customerRepository;

        public CustomerDomain(ICustomerRepository customerRepository)
        {
            this.customerRepository = customerRepository;
        }

        public bool Delete(string customerId)
        {
            return customerRepository.Delete(customerId);
        }

        public async Task<bool> DeleteAsync(string customerId)
        {
            return await customerRepository.DeleteAsync(customerId);
        }

        public Customer Get(string customerId)
        {
            return customerRepository.Get(customerId);
        }

        public IEnumerable<Customer> GetAll()
        {
            return customerRepository.GetAll();
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await customerRepository.GetAllAsync();
        }

        public async Task<Customer> GetAsync(string customerId)
        {
            return await customerRepository.GetAsync(customerId);
        }

        public bool Insert(Customer customer)
        {
            return customerRepository.Insert(customer);
        }

        public async Task<bool> InsertAsync(Customer customer)
        {
            return await customerRepository.InsertAsync(customer);
        }

        public bool Update(Customer customer)
        {
            return customerRepository.Update(customer);
        }

        public async Task<bool> UpdateAsync(Customer customer)
        {
            return await customerRepository.UpdateAsync(customer);
        }
    }
}
