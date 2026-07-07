using BLL.DTOs;
using DAL.Models;
namespace BLL.Services
{
    public interface ISchemeService
    {
        Task<InsuranceSchemeResponseDTO> CreateInsuranceSchemeAsync(InsuranceSchemeDTO scheme);
        Task<InsuranceSchemeResponseDTO> UpdateInsuranceSchemeAsync(int id, InsuranceSchemeDTO scheme);
        Task<List<InsuranceSchemeResponseDTO>> GetAllSchemesAsync();
        Task<InsuranceSchemeResponseDTO?> GetByIdAsync(int id);
        Task<List<InsuranceTypeDTO>> GetAllTypesAsync();
        Task<List<InsuranceSchemeResponseDTO>> GetSchemesByTypeAsync(int insuranceTypeId);
    }
}
