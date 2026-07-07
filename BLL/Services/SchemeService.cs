using AutoMapper;
using Azure.Core;
using BLL.DTOs;
using DAL.Models;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
   public class SchemeService: ISchemeService
    {
        private readonly ISchemeRepository _schemeRepo;
        private readonly IMapper _mapper;
        private readonly IAuditLogRepository _auditRepo;
        public SchemeService(ISchemeRepository schemeRepository, IMapper mapper, IAuditLogRepository auditRepo)
        {
            _schemeRepo = schemeRepository;
            _mapper = mapper;
            _auditRepo = auditRepo;
        }
        public async Task<InsuranceSchemeResponseDTO> CreateInsuranceSchemeAsync(InsuranceSchemeDTO scheme)
        {
            
            
            if (await _schemeRepo.SchemeExistsAsync(scheme.SchemeName))
            {
                return new InsuranceSchemeResponseDTO
                {
                    Message = "Scheme Name Already Exists!",
                    status = false
                };
            }
            var entity = _mapper.Map<InsuranceScheme>(scheme);
            await _schemeRepo.AddSchemeAsync(entity);
            _auditRepo.Log("Scheme", entity.SchemeId, "Created", null);
            var response = _mapper.Map<InsuranceSchemeResponseDTO>(entity);
            response.Message = "Scheme Added Successfully";
            response.status = true;
            return response;
        }

        public async Task<InsuranceSchemeResponseDTO> UpdateInsuranceSchemeAsync(int id, InsuranceSchemeDTO scheme)
        {
            var result = await _schemeRepo.GetInsuranceSchemeByIdAsync(id);
            if (result== null)
            {
                return new InsuranceSchemeResponseDTO
                {
                    SchemeId = id,
                    status = false,
                    Message = "Insurance Scheme not found"
                };
            }

            if (await _schemeRepo.SchemeExistsWithDifferentIdAsync(id, scheme.SchemeName))
                return new InsuranceSchemeResponseDTO
                {
                    status = false,
                    Message = "Scheme name already in use by another scheme."
                };

            _mapper.Map(scheme, result);
            await _schemeRepo.UpdateAsync(result);
            _auditRepo.Log("Scheme", id, "Updated", null);
            var response = _mapper.Map<InsuranceSchemeResponseDTO>(result);
            response.status=true;
            response.Message = "Scheme Updated Successfully";
            return response;

        }

        public async Task<List<InsuranceSchemeResponseDTO>> GetAllSchemesAsync()
        {
            var schemes = await _schemeRepo.ListInsuranceSchemeAsync();
            return _mapper.Map<List<InsuranceSchemeResponseDTO>>(schemes);
        }
        public async Task<InsuranceSchemeResponseDTO?> GetByIdAsync(int id)  // 👈 added
        {
            var scheme = await _schemeRepo.GetInsuranceSchemeByIdAsync(id);
            if (scheme == null) return null;
            return _mapper.Map<InsuranceSchemeResponseDTO>(scheme);
        }

        public async Task<List<InsuranceTypeDTO>> GetAllTypesAsync()
        {
            var types=await _schemeRepo.GetInsuranceTypesAsync();
            return _mapper.Map<List<InsuranceTypeDTO>>(types);
        }
        public async Task<List<InsuranceSchemeResponseDTO>> GetSchemesByTypeAsync(int insuranceTypeId)
        {
            var schemes = await _schemeRepo.GetInsuranceSchemesByInsuranceTypeId(insuranceTypeId);
            return _mapper.Map<List<InsuranceSchemeResponseDTO>>(schemes);
        }
    }
}
