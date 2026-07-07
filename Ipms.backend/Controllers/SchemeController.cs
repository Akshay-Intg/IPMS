using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BLL.DTOs;
using Microsoft.AspNetCore.Authorization;
using BLL.Services;

namespace Ipms.backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SchemeController : ControllerBase
    {
        private readonly ISchemeService _schemeService;
        public SchemeController(ISchemeService schemeService)
        {
            _schemeService = schemeService;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var schemes = await _schemeService.GetAllSchemesAsync();
            if (schemes == null || schemes.Count == 0)
                return NotFound("No schemes found.");
            return Ok(schemes);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var scheme = await _schemeService.GetByIdAsync(id);
            if (scheme == null)
                return NotFound($"Scheme with ID {id} not found.");
            return Ok(scheme);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateInsuranceScheme([FromBody] InsuranceSchemeDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _schemeService.CreateInsuranceSchemeAsync(request);
            if (!result.status)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdateInsuranceScheme([FromRoute] int id, [FromBody] InsuranceSchemeDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var response = await _schemeService.UpdateInsuranceSchemeAsync(id, request);
            if(!response.status)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }
        [HttpGet("allinsurancetype")]
        public async Task<IActionResult> GetAllInsuranceTypeAsync()
        {
            var types=await _schemeService.GetAllTypesAsync();
            if(types==null||types.Count==0)
            {
                return NotFound("No Insurance Types found!");
            }
            return Ok(types);
        }
        [HttpGet("bytype/{insuranceTypeId}")]
        public async Task<IActionResult> GetByType(int insuranceTypeId)
        {
            var schemes = await _schemeService.GetSchemesByTypeAsync(insuranceTypeId);
            return Ok(schemes);
        }
    }
}
