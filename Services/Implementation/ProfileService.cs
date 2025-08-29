using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.DTOs;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        private readonly IMapper _mapper;

        public ProfileService(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;

            // Configure AutoMapper locally (no DI)
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateProfileDto, ApplicationUser>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<ApplicationUser, GetProfileDto>();
                cfg.CreateMap<ApplicationUser, FullProfileDto>();
                // Add mappings for nested entities
                cfg.CreateMap<Certification, CertificationResponseDto>();
                cfg.CreateMap<Education, EducationResponseDto>();
            });
            _mapper = new Mapper(config);
        }

        public async Task<GetProfileDto?> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return _mapper.Map<GetProfileDto>(user);
        }

        public async Task<FullProfileDto?> GetFullProfileAsync(string userId) // New method
        {
            var user = await _context.Users
                .Include(u => u.Certifications.OrderBy(c => c.DisplayOrder).ThenBy(c => c.CreatedAt)) // Include and order
                .Include(u => u.Educations.OrderBy(e => e.DisplayOrder).ThenBy(e => e.CreatedAt)) // Include and order
                                                                                                  // Add more: .Include(u => u.Projects) etc.
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null) return null;
            return _mapper.Map<FullProfileDto>(user);
        }
        public async Task<bool> UpdateProfileAsync(string userId, UpdateProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            // Map the DTO to the user entity
            _mapper.Map(dto, user);
            user.UpdatedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
