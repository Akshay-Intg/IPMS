using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class InsuranceTypeRepository : IInsuranceTypeRepository
    {
        private readonly _IpmsContext _context;
        public InsuranceTypeRepository(_IpmsContext context) => _context = context;

        public List<InsuranceType> GetAll() =>
            _context.InsuranceTypes
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.TypeName)
                    .ToList();

        public InsuranceType? GetById(int id) =>
            _context.InsuranceTypes.Find(id);

        public List<InsuranceScheme> GetSchemesByType(int insuranceTypeId) =>
            _context.InsuranceSchemes
                    .Where(s => s.InsuranceTypeId == insuranceTypeId && s.IsActive == true)
                    .ToList();
    }
}