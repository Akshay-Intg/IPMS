using BLL.DTOs;
using DAL.Models;
namespace BLL.Services
{
    public interface ICustomerService
    {
        Task<CustomerProfileDto> GetByIdAsync(int id);
        Task<List<CustomerDTO>> GetAllAsync();
        Task<CustomerProfileDto> UpdateCustomerAsync(int id,CustomerProfileDto model);
        Task<AdminUpdateCustomerDto?> AdminUpdateCustomerAsync(int id, AdminUpdateCustomerDto model);
        Task<bool> ChangePasswordAsync(int id, ChangePasswordDto model);
        Task<List<Customer>> GetDeletedCustomersAsync();
    }
}
