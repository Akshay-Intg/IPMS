using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
namespace DAL.Repositories
{ 
    public class CustomerRepository : ICustomerRepository
    {
        private readonly _IpmsContext _context;
 
        public CustomerRepository(_IpmsContext context)
        {
            _context = context;
        }
 
        public async Task<bool> UserExistsAsync(string email)
        {
            return await _context.Customers.AnyAsync(c => c.Email==email);
        }
 
        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
           await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
        public async Task<Customer?> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Customers.FindAsync(customerId);
        }
        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            var customer = await _context.Customers
                .Include(c => c.Role)
                .Where(c => c.Email == email)
                .FirstOrDefaultAsync();

            return customer; 
        }
        public async Task<List<Customer>> GetAllCustomersAsync()
        {
            var customerdomain = await _context.Customers.ToListAsync();
            return customerdomain;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
             _context.Update(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public Task<IEnumerable<object>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public Task<List<Customer>> GetDeletedCustomersAsync()
        {
            throw new NotImplementedException();
        }
    }
}
