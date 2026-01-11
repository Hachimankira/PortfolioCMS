using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.DTOs.Experience;
using PortfolioCMS.DTOs;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Interfaces;
using PortfolioCMS.Models.Wrappers;
using PortfolioCMS.Extensions;

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

        public async Task<PagedResult<ExperienceResponseDto>> GetAllExperiencesAsync(string userId, PaginationFilter filter)
        {
            var query = _context.Experiences.Where(e => e.UserId == userId);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                query = query.Where(e => e.Company.Contains(filter.SearchTerm) || e.Position.Contains(filter.SearchTerm) || (e.Description != null && e.Description.Contains(filter.SearchTerm)));
            }

            var totalRecords = await query.CountAsync();

            if (string.IsNullOrEmpty(filter.SortColumn))
            {
                query = query.OrderBy(e => e.DisplayOrder).ThenBy(e => e.CreatedAt);
            }
            else
            {
                query = query.ApplySorting(filter.SortColumn, filter.SortDirection);
            }

            var experiences = await query
                .ApplyPaging(filter.PageNumber, filter.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<ExperienceResponseDto>>(experiences);
            return new PagedResult<ExperienceResponseDto>(dtos, totalRecords);
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