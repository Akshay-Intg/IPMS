namespace Ipms.Frontend.DTOs.Deserialize
{
    public class ErrorLogDeserialize
    {
        public string ErrorMessage { get; set; } = string.Empty;

        public string ShortErrorMessage =>
            ErrorMessage.Length > 100
                ? ErrorMessage.Substring(0, 100) + "..."
                : ErrorMessage;

        public string StackTrace { get; set; } = string.Empty;

        public string LoggedDateFormatted =>
            LoggedDate.ToString("dd MMM yyyy, HH:mm:ss");

        public DateTime LoggedDate { get; set; }

        public string RequestPath { get; set; } = string.Empty;

        public string HttpMethod { get; set; } = string.Empty;

        public string UserName { get; set; } = "System";

        public int StatusCode { get; set; }

        public string StatusClass =>
            StatusCode >= 500 ? "error-server" :
            StatusCode >= 400 ? "error-client" :
            "error-success";
    }
}