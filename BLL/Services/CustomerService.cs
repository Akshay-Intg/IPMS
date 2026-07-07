using AutoMapper;
using Azure.Core;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace BLL.Services
{
    public class CustomerService:ICustomerService
    {
        private readonly ICustomerRepository _repository;
        public readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public CustomerService(ICustomerRepository repository, IConfiguration configuration, IMapper mapper)
        {
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
        }
        public async Task<CustomerProfileDto> GetByIdAsync(int id)
        {
            var customer = await _repository.GetByCustomerIdAsync(id);
            var customerDto = _mapper.Map<CustomerProfileDto>(customer);
            return customerDto;
        }
        public async Task<CustomerProfileDto> UpdateCustomerAsync(int id,CustomerProfileDto model)
        {
            var customer = await _repository.GetByCustomerIdAsync(id);
            if (customer == null) return null;

            _mapper.Map(model, customer); 
            await _repository.UpdateAsync(customer);
            return model;
        }
        public async Task<List<CustomerDTO>> GetAllAsync()
        {
            var customerList = await _repository.GetAllCustomersAsync();

            // Filter out soft deleted customers
            var activeCustomers = customerList
                //.Where(c => c.IsDeleted == false)
                .ToList();

            return _mapper.Map<List<CustomerDTO>>(activeCustomers);
        }

        public async Task<List<Customer>> GetDeletedCustomersAsync()
        {
            return await _repository.GetDeletedCustomersAsync(); // ← NOT _context directly
        }

        public async Task<AdminUpdateCustomerDto?> AdminUpdateCustomerAsync(int id, AdminUpdateCustomerDto model)
        {
            var customer = await _repository.GetByCustomerIdAsync(id);
            if (customer == null) return null;
            if (customer.Email != model.Email)
            {
                var emailExists = await _repository.UserExistsAsync(model.Email);
                if (emailExists)
                    throw new InvalidOperationException("Email is already in use.");
            }

            _mapper.Map(model, customer);
            if(model.IsActive == true)
            {
                customer.IsDeleted = false;
            }
            await _repository.UpdateAsync(customer);
            return model;
        }

        public async Task<bool> ChangePasswordAsync(int id, ChangePasswordDto model)
        {
            var customer = await _repository.GetByCustomerIdAsync(id);
            if (customer == null) return false;

            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, customer.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect.");

            customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await _repository.UpdateAsync(customer);
            return true;
        }
    }
}
