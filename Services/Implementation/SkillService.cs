using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.DTOs.Skill;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Services.Implementation
{
    public class SkillService : ISkillService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SkillService(AppDbContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateSkillDto, Skill>();
                cfg.CreateMap<UpdateSkillDto, Skill>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                        srcMember != null &&
                        (srcMember is not string str || !string.IsNullOrEmpty(str))));  // Ignore null and empty strings
                cfg.CreateMap<Skill, SkillResponseDto>();
            });
            _mapper = new Mapper(config);
        }

        public async Task<IEnumerable<SkillResponseDto>> GetAllSkillsAsync(string userId)
        {
            var skills = await _context.Skills
                .Where(s => s.UserId == userId)
                .OrderBy(s => s.DisplayOrder)
                .ThenBy(s => s.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<SkillResponseDto>>(skills);
        }

        public async Task<SkillResponseDto?> GetSkillByIdAsync(int id, string userId)
        {
            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            return skill == null ? null : _mapper.Map<SkillResponseDto>(skill);
        }

        public async Task<SkillResponseDto> CreateSkillAsync(CreateSkillDto dto, string userId)
        {
            var skill = _mapper.Map<Skill>(dto);
            skill.UserId = userId;
            skill.CreatedAt = DateTime.UtcNow;
            skill.UpdatedAt = DateTime.UtcNow;

            _context.Skills.Add(skill);
            await _context.SaveChangesAsync();
            return _mapper.Map<SkillResponseDto>(skill);
        }

        public async Task<SkillResponseDto?> UpdateSkillAsync(int id, UpdateSkillDto dto, string userId)
        {
            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (skill == null) return null;

            _mapper.Map(dto, skill);  // Only maps non-null/non-empty fields
            skill.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return _mapper.Map<SkillResponseDto>(skill);
        }

        public async Task<bool> DeleteSkillAsync(int id, string userId)
        {
            var skill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId);
            if (skill == null) return false;

            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}