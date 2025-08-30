using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.DTOs.Experience;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Services.Implementation
{
    public class ExperienceService : IExperienceService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ExperienceService(AppDbContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateExperienceDto, Experience>();
                cfg.CreateMap<UpdateExperienceDto, Experience>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                        srcMember != null &&
                        (srcMember is not string str || !string.IsNullOrEmpty(str))));  // Ignore null and empty strings
                cfg.CreateMap<Experience, ExperienceResponseDto>();
            });
            _mapper = new Mapper(config);
        }

        public async Task<IEnumerable<ExperienceResponseDto>> GetAllExperiencesAsync(string userId)
        {
            var experiences = await _context.Experiences
                .Where(e => e.UserId == userId)
                .OrderBy(e => e.DisplayOrder)
                .ThenBy(e => e.CreatedAt)  // Order by CreatedAt if DisplayOrder is the same
                .ToListAsync();
            return _mapper.Map<IEnumerable<ExperienceResponseDto>>(experiences);
        }
        public async Task<ExperienceResponseDto?> GetExperienceByIdAsync(int id, string userId)
        {
            var experience = await _context.Experiences
                .Where(e => e.Id == id && e.UserId == userId)
                .FirstOrDefaultAsync();
            if (experience == null) return null;
            return _mapper.Map<ExperienceResponseDto>(experience);
        }
        public async Task<ExperienceResponseDto> AddExperienceAsync(CreateExperienceDto experienceDto, string userId)
        {
            var experience = _mapper.Map<Experience>(experienceDto);
            experience.CreatedAt = DateTime.UtcNow;
            experience.UpdatedAt = DateTime.UtcNow;
            experience.UserId = userId;
            _context.Experiences.Add(experience);
            await _context.SaveChangesAsync();
            return _mapper.Map<ExperienceResponseDto>(experience);
        }

        public async Task<ExperienceResponseDto?> UpdateExperienceAsync(int id, UpdateExperienceDto experienceDto, string userId)
        {
            if (experienceDto == null || id == 0) return null;

            var experience = await _context.Experiences
                .Where(e => e.Id == id && e.UserId == userId)
                .FirstOrDefaultAsync();
            if (experience == null) return null;

            _mapper.Map(experienceDto, experience);
            experience.UpdatedAt = DateTime.UtcNow;

            _context.Experiences.Update(experience);
            await _context.SaveChangesAsync();
            return _mapper.Map<ExperienceResponseDto>(experience);
        }

        public async Task<bool> DeleteExperienceAsync(int id, string userId)
        {
            var experience = await _context.Experiences
                .Where(e => e.Id == id && e.UserId == userId)
                .FirstOrDefaultAsync();
            if (experience == null) return false;
            _context.Experiences.Remove(experience);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}