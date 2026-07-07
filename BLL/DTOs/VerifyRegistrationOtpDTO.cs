namespace BLL.DTOs
{
    public class VerifyRegistrationOtpDTO
    {
        public string Email { get; set; } = null!;
        public string OTP { get; set; } = null!;
    }
}