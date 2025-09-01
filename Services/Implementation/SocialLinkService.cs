using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.Models;
using PortfolioCMS.Models.DTOs;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Services.Implementation
{
    public class SocialLinksService : ISocialLinksService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public SocialLinksService(AppDbContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CreateLinkDto, SocialLinks>();
                cfg.CreateMap<UpdateLinkDto, SocialLinks>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                        srcMember != null &&
                        (srcMember is not string str || !string.IsNullOrEmpty(str))));  // Ignore null and empty strings
                cfg.CreateMap<SocialLinks, LinkResponseDto>();
            });
            _mapper = new Mapper(config);
        }

        public async Task<IEnumerable<LinkResponseDto>> GetAllAsync(string userId)
        {
            var links = await _context.SocialLinks
                .Where(l => l.UserId == userId)
                .OrderBy(l => l.DisplayOrder)
                .ThenBy(l => l.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<LinkResponseDto>>(links);
        }

        public async Task<LinkResponseDto?> GetByIdAsync(int id, string userId)
        {
            var link = await _context.SocialLinks
                .Where(l => l.Id == id && l.UserId == userId)
                .FirstOrDefaultAsync();
            if (link == null) return null;
            return _mapper.Map<LinkResponseDto>(link);
        }

        public async Task<LinkResponseDto> CreateAsync(CreateLinkDto dto, string userId)
        {
            var link = _mapper.Map<SocialLinks>(dto);
            link.CreatedAt = DateTime.UtcNow;
            link.UpdatedAt = DateTime.UtcNow;
            link.UserId = userId;
            _context.SocialLinks.Add(link);
            await _context.SaveChangesAsync();
            return _mapper.Map<LinkResponseDto>(link);
        }

        public async Task<LinkResponseDto?> UpdateAsync(UpdateLinkDto dto, string userId)
        {
            if (dto == null || dto.Id == 0) return null;

            var existingLink = await _context.SocialLinks
                .Where(l => l.Id == dto.Id && l.UserId == userId)
                .FirstOrDefaultAsync();
            if (existingLink == null) return null;

            // Map updated fields from DTO to the existing entity
            _mapper.Map(dto, existingLink);
            await _context.SaveChangesAsync();
            return _mapper.Map<LinkResponseDto>(existingLink);
        }
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var link = await _context.SocialLinks
                .Where(l => l.Id == id && l.UserId == userId)
                .FirstOrDefaultAsync();
            if (link == null) return false;

            _context.SocialLinks.Remove(link);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}