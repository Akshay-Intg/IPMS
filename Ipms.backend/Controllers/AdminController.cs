using BLL.DTOs;
using BLL.Services;
using DAL.Data;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ipms.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly _IpmsContext _context;
        private readonly IAuthService _authService;
        private readonly ICustomerService _customerService;

        public AdminController(_IpmsContext context, IAuthService authService, ICustomerService customerService)
        {
            _context = context;
            _authService = authService;
            _customerService = customerService;
        }

        [HttpGet("all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll()
        {
            var responseList = await _customerService.GetAllAsync();
            if (responseList.Count == 0)
            {
                return NotFound();
            }
            return Ok(responseList);
        }

        [HttpGet("deleted")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDeletedCustomers()
        {
            var customers = await _customerService.GetDeletedCustomersAsync();
            return Ok(customers);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "Admin")]   
        public async Task<IActionResult> AdminUpdateCustomer([FromRoute] int id, [FromBody] AdminUpdateCustomerDto customerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _customerService.AdminUpdateCustomerAsync(id, customerDto);
            if (result == null)
                throw new KeyNotFoundException($"Customer with id {id} not found");

            return Ok(result);
        }
        
        [HttpPost("create-broker")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateBroker([FromBody] CreateBrokerDto dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid data." });

            var exists = _context.Customers.Any(c => c.Email == dto.Email);
            if (exists)
                return BadRequest(new { message = "Email already registered." });

            var brokerRole = _context.Roles
                .FirstOrDefault(r => r.RoleName == "Broker");

            if (brokerRole == null)
                return BadRequest(new { message = "Broker role not found in DB." });

            var broker = new Customer
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                PhoneNo = dto.PhoneNo,
                RoleId = brokerRole.RoleId,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            _context.Customers.Add(broker);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Broker created successfully.", brokerId = broker.CustomerId });
        }
        // GET: api/Admin/brokers
        [HttpGet("brokers")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllBrokers()
        {
            var brokers = _context.Customers
                .Include(c => c.Role)
                .Where(c => c.Role.RoleName == "Broker")
                .Select(c => new
                {
                    BrokerId = c.CustomerId,
                    FullName = c.FirstName + " " + c.LastName,
                    Email = c.Email,
                    PhoneNo = c.PhoneNo,
                    LicenseNumber = c.LicenseNumber,
                    IsActive = c.IsActive,
                    CreatedDate = c.CreatedDate,
                    TotalPoliciesSold = _context.Policies
                                                .Count(p => p.BrokerId == c.CustomerId),
                    TotalPremiumCollected = _context.Policies
                                                .Where(p => p.BrokerId == c.CustomerId)
                                                .Sum(p => (decimal?)p.PremiumAmount) ?? 0
                })
                .ToList();

            return Ok(brokers);
        }

        [HttpPut("toggle-broker/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleBrokerStatus(int id)
        {
            var broker = await _context.Customers.FindAsync(id);

            if (broker == null)
                return NotFound(new { message = "Broker not found." });

            broker.IsActive = !broker.IsActive;
            await _context.SaveChangesAsync();

            bool isNowActive = (bool)broker.IsActive; 

            return Ok(new
            {
                message = "Broker " + (isNowActive ? "activated" : "deactivated") + " successfully.",
                isActive = isNowActive
            });
        }

        [HttpPut("soft-delete/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _context.Customers.Find(id);

            if (customer == null)
            {
                return NotFound(new { message = "Customer not found" });
            }

            try
            {
                _context.Customers.Remove(customer);
                _context.SaveChanges();

                return Ok(new { message = "Customer deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("restore/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RestoreCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                return NotFound(new { message = "Customer not found." });

            customer.IsDeleted = false;
            customer.IsActive = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Customer restored successfully." });
        }
    }
}
