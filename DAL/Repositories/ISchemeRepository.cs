using DAL.Models;
using DAL.Data;
namespace DAL.Repositories
{
    public interface ISchemeRepository
    {
        Task<List<InsuranceScheme>> ListInsuranceSchemeAsync();
        Task<bool> SchemeExistsAsync(string name);
        Task<bool> SchemeExistsWithDifferentIdAsync(int id, string name);
        Task<InsuranceScheme> AddSchemeAsync(InsuranceScheme Scheme);
        Task<InsuranceScheme?> GetInsuranceSchemeByIdAsync(int schemeId);
        Task<InsuranceScheme> UpdateAsync(InsuranceScheme Scheme);
        Task<List<InsuranceType>> GetInsuranceTypesAsync();
        Task<List<InsuranceScheme>> GetInsuranceSchemesByInsuranceTypeId(int insuranceTypeId);
    }
}
