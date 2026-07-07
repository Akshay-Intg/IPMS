using DAL.Models;
using DAL.Data;
namespace DAL.Repositories
{
    public interface ICustomerRepository
    {
        Task<bool> UserExistsAsync(string email);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer?> GetByCustomerIdAsync(int id);
        Task<Customer?> GetCustomerByEmailAsync(string Email);
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Customer> UpdateAsync(Customer customer);
        Task<IEnumerable<object>> GetAllAsync();
        Task UpdateCustomerAsync(Customer customer);
        Task<List<Customer>> GetDeletedCustomersAsync();
    }
}
