using DAL.Models;

namespace DAL.Repositories
{
    public interface IInsuranceTypeRepository
    {
        List<InsuranceType> GetAll();
        InsuranceType? GetById(int id);
        List<InsuranceScheme> GetSchemesByType(int insuranceTypeId);
    }
}