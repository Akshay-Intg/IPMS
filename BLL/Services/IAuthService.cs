using BLL.DTOs;

namespace BLL.Services
{
    public interface IAuthService
    {
        Task<ResponseDto> RegisterCustomerAsync(RegisterDto request);
        Task<LoginResponseDTO> LoginAsync(LoginDTO request);
        string GenerateAndSendOTP(string email);
        bool VerifyOTPAndResetPassword(VerifyOtpDTO dto);
        Task<ResponseDto> InitiateRegistrationAsync(RegisterDto request);
        bool VerifyRegistrationOTP(VerifyRegistrationOtpDTO dto);
    }
}
