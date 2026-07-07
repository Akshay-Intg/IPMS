using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ipms.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClaimsController : ControllerBase
    {
        private readonly IClaimsService claimsService;
        public ClaimsController(IClaimsService claimsService)
        {
           this.claimsService = claimsService;
        }

        [HttpPost("apply")]
        public async Task<IActionResult> ApplyClaim([FromBody] CreateClaimDTO request)
        {
            if (request == null)
            {
                return NotFound();
            }
            var response=await claimsService.ApplyClaimAsync(request);
            if (response.ClaimId==0)
            {
                return Conflict(response);
            }else if (response.ClaimId == -1)
            {
                return UnprocessableEntity(response);
            }
            return Ok(response);
        }

        [HttpGet("claimslist")]
        public async Task<IActionResult> GetClaimsById()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return Unauthorized("Invalid Token");
            }
            var response=await claimsService.GetClaimsAsync(userId);
            if (response.Count() == 0) 
            {
                return Ok("Claims not available!");
            }
            return Ok(response);

        }

        [HttpGet("allclaims")]
        public async Task<IActionResult> GetClaimsList()
        {
            var response=await claimsService.GetAllClaimsAsync();
            if(response.Count() == 0)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpGet("claimByClaimId/{claimId}")]
        public async Task<IActionResult> GetClaimsByClaimId(int claimId)
        {
            var response=await claimsService.GetClaimByIdAsync(claimId);
            if(response == null)
            {
                return NotFound(); 
            }
            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateClaimStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return BadRequest("Status cannot be empty.");

            var result = await claimsService.UpdateClaimStatusAsync(id, status);

            if (!result)
                return NotFound($"Claim with ID {id} not found.");

            return Ok("Claim status updated successfully.");
        }

    }
}
