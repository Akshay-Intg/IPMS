using AutoMapper;
using BLL.DTOs;
using DAL.Data;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Claim=System.Security.Claims.Claim;


namespace BLL.Services
{
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _repository;
        public readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IAuditLogRepository _auditRepo;
        public AuthService(ICustomerRepository repository,IConfiguration configuration,IMapper mapper, IAuditLogRepository auditRepo)
        {
            _repository = repository;
            _configuration = configuration;
            _mapper = mapper;
            _auditRepo = auditRepo;
        }

        public async Task<ResponseDto> RegisterCustomerAsync(RegisterDto request)
        {
            var response = new ResponseDto();
            if (await _repository.UserExistsAsync(request.Email))
            {
                response.message = "Email Already Exists!";
                response.status = false;
                return response;
            }
            var newCustomer = _mapper.Map<Customer>(request);
            newCustomer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            await _repository.AddCustomerAsync(newCustomer);
            _auditRepo.Log("Customer", newCustomer.CustomerId, "Registered", newCustomer.CustomerId);
            response = _mapper.Map<ResponseDto>(newCustomer);
            response.status = true;
            response.message = "Registration Successful";

            return response;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO request)
        {
            var user = await _repository.GetCustomerByEmailAsync(request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return new LoginResponseDTO { Message = "Invalid Email or Password." };

            if (user.IsActive == false)
                return new LoginResponseDTO { Message = "Account is Deactivated. Contact Admin" };

            // ← Add null check for Role
            if (user.Role == null)
                return new LoginResponseDTO { Message = "User role not assigned. Contact Admin." };
            _auditRepo.Log("Customer", user.CustomerId, "Login", user.CustomerId);
            var token = GenerateJwtToken(user);
            var response = _mapper.Map<LoginResponseDTO>(user);
            response.Token = token;
            response.Message = "Login Successful";
            response.Role = user.Role.RoleName; // ← safe now

            return response;
        }

        private string GenerateJwtToken(Customer user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]!);

            var claims = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, user.CustomerId.ToString()),
                 new Claim(ClaimTypes.Email, user.Email),
                 new Claim(ClaimTypes.Name,user.Email),
                 new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"]!)),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateAndSendOTP(string email)
        {
            var customer = _repository.GetCustomerByEmailAsync(email).Result;
            if (customer == null)
                throw new Exception("No account found with this email.");

            var otp = new Random().Next(100000, 999999).ToString();

            customer.OTP = otp;
            customer.OTPExpiry = DateTime.Now.AddMinutes(10); 
            _repository.UpdateCustomerAsync(customer).Wait();

            return otp;
        }

        public bool VerifyOTPAndResetPassword(VerifyOtpDTO dto)
        {
            var customer = _repository.GetCustomerByEmailAsync(dto.Email).Result;
            if (customer == null)
                throw new Exception("No account found with this email.");

            if (customer.OTP != dto.OTP)
                throw new Exception("Invalid OTP.");

            if (customer.OTPExpiry < DateTime.Now)
                throw new Exception("OTP has expired. Please request a new one.");

            customer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);
            customer.OTP = null;
            customer.OTPExpiry = null;
            _repository.UpdateCustomerAsync(customer).Wait();

            return true;
        }



        public async Task<ResponseDto> InitiateRegistrationAsync(RegisterDto request)
        {
            var response = new ResponseDto();

            if (await _repository.UserExistsAsync(request.Email))
            {
                response.message = "Email Already Exists!";
                response.status = false;
                return response;
            }

            var newCustomer = _mapper.Map<Customer>(request);
            newCustomer.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            newCustomer.IsActive = false;

            var otp = new Random().Next(100000, 999999).ToString();
            newCustomer.OTP = otp;
            newCustomer.OTPExpiry = DateTime.Now.AddMinutes(10);

            await _repository.AddCustomerAsync(newCustomer);

            response.status = true;
            response.message = "OTP_REQUIRED";
            response.CustomerId = newCustomer.CustomerId; 

            return response;
        }

        public bool VerifyRegistrationOTP(VerifyRegistrationOtpDTO dto)
        {
            var customer = _repository.GetCustomerByEmailAsync(dto.Email).Result;
            if (customer == null)
                throw new Exception("No account found with this email.");

            if (customer.OTP != dto.OTP)
                throw new Exception("Invalid OTP.");

            if (customer.OTPExpiry < DateTime.Now)
                throw new Exception("OTP has expired. Please register again.");

            customer.IsActive = true;
            customer.OTP = null;
            customer.OTPExpiry = null;
            _repository.UpdateCustomerAsync(customer).Wait();

            _auditRepo.Log("Customer", customer.CustomerId, "Registered", customer.CustomerId);

            return true;
        }
    }
}

