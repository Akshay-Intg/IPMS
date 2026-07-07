using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.DTOs
{
    public class PolicyRequestDTO
    {
        public int SchemeId { get; set; }
        public string? InsuranceType { get; set; }
        public string? FullName { get; set; }
        public string? MaritalStatus { get; set; }
        public string? Gender { get; set; }
        public string? PANCard { get; set; }
        public DateTime DOB { get; set; }
        public int HeightFeet { get; set; }
        public int HeightInches { get; set; }
        public decimal Weight { get; set; }
        public string? HouseNo { get; set; }
        public string? Street { get; set; }
        public string? City { get; set; }
        public string? PinCode { get; set; }
        public bool Smokes { get; set; }
        public string? MedicalHistory { get; set; }
        public string? NomineeName { get; set; }
        public string? NomineeRelationship { get; set; }
        public decimal? SumAssured { get; set; }
        public int? PolicyTerm { get; set; }
        public string? OrganizationName { get; set; }
        public int? MemberCount { get; set; }
    }
}
