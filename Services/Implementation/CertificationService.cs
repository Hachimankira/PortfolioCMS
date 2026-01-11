using PortfolioCMS.DTOs;
using PortfolioCMS.Models;
using PortfolioCMS.Models.Wrappers;
using PortfolioCMS.Extensions;
using PortfolioCMS.Services.Interfaces;
using AutoMapper;
using PortfolioCMS.Data;
using Microsoft.EntityFrameworkCore;

namespace PortfolioCMS.Services.Implementation
{
    public class CertificationService : ICertification
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public CertificationService(AppDbContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Certification, CertificationResponseDto>();
                cfg.CreateMap<CreateCertificationDto, Certification>();
                cfg.CreateMap<UpdateCertificationDto, Certification>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            });
            _mapper = new Mapper(config);
        }

        public async Task<PagedResult<CertificationResponseDto>> GetAllAsync(string userId, PaginationFilter filter)
        {
            var query = _context.Certifications.Where(c => c.UserId == userId);
            
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(c => c.Title.Contains(filter.SearchTerm) || c.Issuer.Contains(filter.SearchTerm));
            }
            
            var totalRecords = await query.CountAsync();

            if (string.IsNullOrEmpty(filter.SortColumn))
            {
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.CreatedAt);
            }
            else
            {
                query = query.ApplySorting(filter.SortColumn, filter.SortDirection);
            }

            var certifications = await query
                .ApplyPaging(filter.PageNumber, filter.PageSize)
                .ToListAsync();
                
            var dtos = _mapper.Map<IEnumerable<CertificationResponseDto>>(certifications);
            return new PagedResult<CertificationResponseDto>(dtos, totalRecords);
        }

        public async Task<CertificationResponseDto?> GetByIdAsync(int id, string userId)
        {
            var certification = await _context.Certifications
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (certification == null) return null;
            return _mapper.Map<CertificationResponseDto>(certification);
        }

        public async Task<CertificationResponseDto> CreateAsync(CreateCertificationDto dto, string userId)
        {
            // Validate user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                throw new ArgumentException("User not found");
            }
            var certification = _mapper.Map<Certification>(dto);
            certification.UserId = userId;
            certification.CreatedAt = DateTime.UtcNow;
            certification.UpdatedAt = DateTime.UtcNow;

            _context.Certifications.Add(certification);
            await _context.SaveChangesAsync();

            return _mapper.Map<CertificationResponseDto>(certification);
        }

        public async Task<CertificationResponseDto?> UpdateAsync(UpdateCertificationDto dto, int id, string userId)
        {
            var certification = await _context.Certifications
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (certification == null) return null;

            _mapper.Map(dto, certification);
            certification.UpdatedAt = DateTime.UtcNow;

            _context.Certifications.Update(certification);
            await _context.SaveChangesAsync();
            return _mapper.Map<CertificationResponseDto>(certification);
        }
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var certification = await _context.Certifications
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);

            if (certification == null)
            {
                return false;
            }

            _context.Certifications.Remove(certification);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}