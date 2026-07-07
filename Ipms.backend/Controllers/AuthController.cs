using BLL.DTOs;
using BLL.Services;
using DAL.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Ipms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly EmailService _emailService;

        public AuthController(IAuthService authService, EmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }

        [HttpPost("register")]
        public async  Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var result = await _authService.RegisterCustomerAsync(request);
            if (result.CustomerId == 0)
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            var result = await _authService.LoginAsync(model);
            if (result.CustomerId == 0)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("secure-data")]
        public IActionResult SecureData()
        {
            return Ok("JWT Working Successfully");
        }
        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public IActionResult ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            try
            {
                var otp = _authService.GenerateAndSendOTP(dto.Email);

                var body = $@"
        <div style='font-family:Poppins,sans-serif; max-width:480px; margin:auto;
                    border:1px solid #eee; border-radius:10px; overflow:hidden;'>
            <div style='background:#2c3e50; padding:24px; text-align:center;'>
                <h2 style='color:#fff; margin:0;'>IPMS Password Reset</h2>
            </div>
            <div style='padding:32px;'>
                <p style='color:#333;'>You requested a password reset. Use the OTP below:</p>
                <div style='text-align:center; margin:24px 0;'>
                    <span style='font-size:36px; font-weight:800; letter-spacing:10px;
                                 color:#2c3e50; background:#f0f4f8; padding:16px 32px;
                                 border-radius:10px; display:inline-block;'>
                        {otp}
                    </span>
                </div>
                <p style='color:#e94560; font-weight:600; text-align:center;'>
                    ⏱ This OTP expires in 10 minutes.
                </p>
                <p style='color:#aaa; font-size:12px; text-align:center; margin-top:24px;'>
                    If you did not request this, please ignore this email.
                </p>
            </div>
        </div>";

                _emailService.SendEmail(dto.Email, "IPMS - Your Password Reset OTP", body);
                return Ok(new { message = "OTP sent to your email." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("verify-otp")]
        [AllowAnonymous]
        public IActionResult VerifyOTP([FromBody] VerifyOtpDTO dto)
        {
            try
            {
                _authService.VerifyOTPAndResetPassword(dto);
                return Ok(new { message = "Password reset successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpPost("send-contact")]
        [AllowAnonymous]
        public IActionResult SendContact([FromBody] ContactDTO dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.FullName) ||
                    string.IsNullOrWhiteSpace(dto.Email) ||
                    string.IsNullOrWhiteSpace(dto.Message))
                    return BadRequest(new { message = "All fields are required." });

                _emailService.SendContactEmail(dto);
                return Ok(new { message = "Message sent successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("initiate-register")]
        [AllowAnonymous]
        public async Task<IActionResult> InitiateRegister([FromBody] RegisterDto request)
        {
            try
            {
                var result = await _authService.InitiateRegistrationAsync(request);

                if (!result.status)
                    return BadRequest(new { message = result.message });

                var customer = result;
                var otp = _authService.GenerateAndSendOTP(request.Email);

                var body = $@"
        <div style='font-family:Poppins,sans-serif; max-width:480px; margin:auto;
                    border:1px solid #eee; border-radius:10px; overflow:hidden;'>
            <div style='background:#2c3e50; padding:24px; text-align:center;'>
                <h2 style='color:#fff; margin:0;'>Welcome to IPMS!</h2>
            </div>
            <div style='padding:32px;'>
                <p style='color:#333;'>Hi <strong>{request.FirstName}</strong>,</p>
                <p style='color:#333;'>Thank you for registering. 
                   Please verify your email using the OTP below:</p>
                <div style='text-align:center; margin:24px 0;'>
                    <span style='font-size:36px; font-weight:800; letter-spacing:10px;
                                 color:#2c3e50; background:#f0f4f8; padding:16px 32px;
                                 border-radius:10px; display:inline-block;'>
                        {otp}
                    </span>
                </div>
                <p style='color:#e94560; font-weight:600; text-align:center;'>
                    ⏱ This OTP expires in 10 minutes.
                </p>
                <p style='color:#aaa; font-size:12px; text-align:center; margin-top:24px;'>
                    If you did not register on IPMS, please ignore this email.
                </p>
            </div>
        </div>";

                _emailService.SendEmail(request.Email, "IPMS — Verify Your Email", body);

                return Ok(new { message = "OTP sent to your email.", email = request.Email });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("verify-registration")]
        [AllowAnonymous]
        public IActionResult VerifyRegistration([FromBody] VerifyRegistrationOtpDTO dto)
        {
            try
            {
                _authService.VerifyRegistrationOTP(dto);
                return Ok(new { message = "Email verified! Account activated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

