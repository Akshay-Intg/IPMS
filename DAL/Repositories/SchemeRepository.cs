using DAL.Data;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
namespace DAL.Repositories
{
    public class SchemeRepository: ISchemeRepository
    {
        private readonly _IpmsContext _context;

        public SchemeRepository(_IpmsContext context)
        {
            _context = context;
        }
        public async Task<List<InsuranceScheme>> ListInsuranceSchemeAsync()
        {
            return await _context.InsuranceSchemes.ToListAsync();
        }
        public async Task<bool> SchemeExistsAsync(string Name)
        {
            return await _context.InsuranceSchemes.AnyAsync(c => c.SchemeName.ToLower() == Name.ToLower());
        }

        public async Task<bool> SchemeExistsWithDifferentIdAsync(int id, string name)
        {
            return await _context.InsuranceSchemes
                .AnyAsync(c => c.SchemeName.ToLower() == name.ToLower() && c.SchemeId != id);
        }

        public async Task<InsuranceScheme> AddSchemeAsync(InsuranceScheme Scheme)
        {
            await _context.InsuranceSchemes.AddAsync(Scheme);
            await _context.SaveChangesAsync();
            return Scheme;
        }

        public async Task<InsuranceScheme?> GetInsuranceSchemeByIdAsync(int schemeId)
        {
            return await _context.InsuranceSchemes.FirstOrDefaultAsync(s => s.SchemeId == schemeId);
        }

        public async Task<InsuranceScheme> UpdateAsync(InsuranceScheme Scheme)
        {
            _context.Update(Scheme);
            await _context.SaveChangesAsync();
            return Scheme;
        }

        public async Task<List<InsuranceType>> GetInsuranceTypesAsync()
        {
            return await _context.InsuranceTypes.ToListAsync();
        }
        public async Task<List<InsuranceScheme>> GetInsuranceSchemesByInsuranceTypeId(int insuranceTypeId)
        {
            return await _context.InsuranceSchemes
                 .Where(x => x.InsuranceTypeId == insuranceTypeId&&x.IsActive==true).ToListAsync();
        }
    }
}
