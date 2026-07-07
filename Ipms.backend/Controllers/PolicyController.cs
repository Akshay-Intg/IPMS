using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ipms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public PolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        [HttpPost("apply")]
        public IActionResult Apply([FromBody] PolicyRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var customerId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            try
            {
                int policyId = _policyService.CreatePolicy(dto, customerId);
                return Ok(new
                {
                    success = true,
                    message = "Policy applied successfully.",
                    policyId = policyId,
                    premium = Math.Round((dto.SumAssured ?? 0) * 0.05m, 2)
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("my-policies")]
        public IActionResult GetMyPolicies()
        {
            var customerId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var policies = _policyService.GetPoliciesByCustomer(customerId);
            return Ok(policies);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var policy = _policyService.GetPolicy(id);
            if (policy == null)
                return NotFound(new { error = "Policy not found." });
            return Ok(policy);
        }

        [HttpGet("all")]
        [Authorize(Roles = "Underwriter")]
        public IActionResult GetAll()
        {
            var policies = _policyService.GetAllPolicies();
            return Ok(policies);
        }


        [HttpGet("underwriter/{id}")]
        [Authorize(Roles = "Underwriter")]
        public IActionResult GetForUnderwriter(int id)
        {
            var policy = _policyService.GetPolicyForUnderwriter(id);
            if (policy == null)
                return NotFound(new { error = "Policy not found." });
            return Ok(policy);
        }

        [HttpPut("review/{id}")]
        [Authorize(Roles = "Underwriter")]
        public IActionResult Review(int id, [FromBody] UnderwriterReviewDTO dto)
        {
            try
            {
                var result = _policyService.ReviewPolicy(id, dto);
                if (!result)
                    return NotFound(new { error = "Policy not found." });
                return Ok(new { message = $"Policy {dto.Status} successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}