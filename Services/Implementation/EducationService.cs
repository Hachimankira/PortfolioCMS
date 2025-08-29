using PortfolioCMS.DTOs;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Interfaces;
using AutoMapper;
using PortfolioCMS.Data;
using Microsoft.EntityFrameworkCore;

namespace PortfolioCMS.Services.Implementation
{
    public class EducationService : IEducationService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public EducationService(AppDbContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateEducationDto, Education>();
                cfg.CreateMap<UpdateEducationDto, Education>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<Education, EducationResponseDto>();
            });
            _mapper = new Mapper(config);
        }
        public async Task<IEnumerable<EducationResponseDto>> GetAllAsync(string userId)
        {
            var educations = await _context.Educations
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.DisplayOrder)
                .ThenBy(e => e.CreatedAt)  // Order by CreatedAt if DisplayOrder is the same
                .ToListAsync();
            return _mapper.Map<IEnumerable<EducationResponseDto>>(educations);
        }
        public async Task<EducationResponseDto?> GetByIdAsync(int id, string userId)
        {
            var education = await _context.Educations
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (education == null) return null;
            return _mapper.Map<EducationResponseDto>(education);
        }
        public async Task<EducationResponseDto> CreateAsync(CreateEducationDto dto, string userId)
        {
            var education = _mapper.Map<Education>(dto);
            education.UserId = userId;
            education.CreatedAt = DateTime.UtcNow;
            education.UpdatedAt = DateTime.UtcNow;
            _context.Educations.Add(education);

            await _context.SaveChangesAsync();
            return _mapper.Map<EducationResponseDto>(education);
        }

        public async Task<EducationResponseDto?> UpdateAsync(int id, UpdateEducationDto dto, string userId)
        {
            var education = await _context.Educations
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (education == null) return null;

            _mapper.Map(dto, education);
            education.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return _mapper.Map<EducationResponseDto>(education);
        }
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var education = await _context.Educations
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);
            if (education == null) return false;

            _context.Educations.Remove(education);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}