using PortfolioCMS.DTOs;
using PortfolioCMS.Models;
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

        public async Task<IEnumerable<CertificationResponseDto>> GetAllAsync(string userId)
        {
            var certifications = await _context.Certifications
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.CreatedAt)  // Order by CreatedAt if DisplayOrder is the same
                .ToListAsync();
            return _mapper.Map<IEnumerable<CertificationResponseDto>>(certifications);
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

        public async Task<bool> UpdateAsync(UpdateCertificationDto dto, int id, string userId)
        {
            var certification = await _context.Certifications
                .FirstOrDefaultAsync(c => c.Id == id && c.UserId == userId);
            if (certification == null) return false;

            _mapper.Map(dto, certification);
            certification.UpdatedAt = DateTime.UtcNow;

            _context.Certifications.Update(certification);
            await _context.SaveChangesAsync();
            return true;
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