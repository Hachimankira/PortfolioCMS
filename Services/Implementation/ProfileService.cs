using AutoMapper;
using Microsoft.AspNetCore.Identity;
using PortfolioCMS.DTOs;
using PortfolioCMS.Models;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Services.Implementation
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public ProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;

            // Configure AutoMapper locally (no DI)
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UpdateProfileDto, ApplicationUser>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                cfg.CreateMap<ApplicationUser, GetProfileDto>();
            });
            _mapper = new Mapper(config);
        }

        public async Task<GetProfileDto?> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return _mapper.Map<GetProfileDto>(user);
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
