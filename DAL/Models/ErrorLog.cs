using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class ErrorLog
{
    public int ErrorId { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public string? StackTrace { get; set; }

    public DateTime? LoggedDate { get; set; }
    public string? RequestPath {  get; set; }
    public string? HttpMethod { get; set; } 
    public string? UserName {  get; set; }  
    public int? StatusCode {  get; set; }
}
