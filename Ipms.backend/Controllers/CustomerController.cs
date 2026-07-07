using BLL.DTOs;
using BLL.Services;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System.Security.Claims;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomerController(ICustomerRepository repository, ISchemeRepository schemeRepository,
    ICustomerService customerService, IPaymentService paymentService,IPolicyRepository policyRepository) : Controller
{
    private readonly ICustomerRepository _repository = repository;
    private readonly ISchemeRepository _schemeRepo = schemeRepository;
    private readonly ICustomerService _customerService = customerService;
    private readonly IPaymentService _paymentService = paymentService;
    private readonly IPolicyRepository _repo=policyRepository;

    
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
        {
            return Unauthorized("Invalid Token");
        }

        var customer = await _repository.GetByCustomerIdAsync(userId);

        if (customer == null) return NotFound("User not found");

        return Ok(new
        {
            customer.CustomerId,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.PhoneNo,
            customer.DateOfBirth
        });
    }
    [HttpGet("InsuranceSchemes")]
    public async Task<IActionResult> GetInsuranceSchemes()
    {
        var SchemeList = await _schemeRepo.ListInsuranceSchemeAsync();
        if(SchemeList.Count == 0)
        {
            return NotFound();
        }
        return Ok(SchemeList);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute]int id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer == null)
            throw new KeyNotFoundException($"Customer with id {id} not found");

        return Ok(customer);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateCustomer([FromRoute] int id,[FromBody] CustomerProfileDto customerProfileDto)
    {
        var customer=await _customerService.UpdateCustomerAsync(id, customerProfileDto);
        if (customer == null)
            return NotFound();
        return Ok(customer);
    }

    [HttpPut("change-password/{id}")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromRoute] int id, [FromBody] ChangePasswordDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        try
        {
            var result = await _customerService.ChangePasswordAsync(id, dto);
            if (!result) return NotFound("Customer not found.");
            return Ok("Password changed successfully.");
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult Payment(int policyId, decimal amount, string type, string plan)
    {
        ViewBag.PolicyId = policyId;
        ViewBag.Amount = amount;
        ViewBag.Type = type;
        ViewBag.Plan = plan;
        return View();
    }

    [HttpPost("ConfirmPayment")]
    [IgnoreAntiforgeryToken]
    public IActionResult ConfirmPayment([FromBody] PaymentConfirmRequest request)
    {
        try
        {
            _paymentService.ProcessPayment(request.PolicyId, request.Amount, request.PaymentMode);
            return Ok(new { success = true });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { success = false, error = ex.Message });
        }
    }
}

