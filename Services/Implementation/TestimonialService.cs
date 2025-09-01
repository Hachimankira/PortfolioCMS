using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PortfolioCMS.Data;
using PortfolioCMS.Models;
using PortfolioCMS.Models.DTOs;
using PortfolioCMS.Services.Interfaces;

namespace PortfolioCMS.Services.Implementation
{
    public class TestimonialService : ITestimonialService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TestimonialService(AppDbContext context)
        {
            _context = context;
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Testimonial, TestimonialResponseDto>();
                cfg.CreateMap<CreateTestimonialDto, Testimonial>();
                cfg.CreateMap<UpdateTestimonialDto, Testimonial>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                        srcMember != null &&
                        (srcMember is not string str || !string.IsNullOrEmpty(str)))); ;
            });
            _mapper = config.CreateMapper();
        }

        public async Task<IEnumerable<TestimonialResponseDto>> GetAllAsync(string userId)
        {
            var testimonials = await _context.Testimonials
                .Where(t => t.UserId == userId)
                .OrderBy(t => t.DisplayOrder)
                .ThenBy(t => t.CreatedAt)
                .ToListAsync();
            return _mapper.Map<IEnumerable<TestimonialResponseDto>>(testimonials);
        }

        public async Task<TestimonialResponseDto?> GetByIdAsync(int id, string userId)
        {
            var testimonial = await _context.Testimonials
                .Where(t => t.Id == id && t.UserId == userId)
                .FirstOrDefaultAsync();
            if (testimonial == null) return null;
            return _mapper.Map<TestimonialResponseDto>(testimonial);
        }
        public async Task<TestimonialResponseDto> CreateAsync(CreateTestimonialDto dto, string userId)
        {
            var testimonial = _mapper.Map<Testimonial>(dto);
            testimonial.UserId = userId;
            testimonial.CreatedAt = DateTime.UtcNow;
            testimonial.UpdatedAt = DateTime.UtcNow;
            _context.Testimonials.Add(testimonial);
            await _context.SaveChangesAsync();
            return _mapper.Map<TestimonialResponseDto>(testimonial);
        }
        public async Task<TestimonialResponseDto?> UpdateAsync(UpdateTestimonialDto dto, string userId)
        {
            var testimonial = await _context.Testimonials
                .Where(t => t.Id == dto.Id && t.UserId == userId)
                .FirstOrDefaultAsync();
            if (testimonial == null) return null;

            _mapper.Map(dto, testimonial);
            testimonial.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return _mapper.Map<TestimonialResponseDto>(testimonial);
        }
        public async Task<bool> DeleteAsync(int id, string userId)
        {
            var testimonial = await _context.Testimonials
                .Where(t => t.Id == id && t.UserId == userId)
                .FirstOrDefaultAsync();
            if (testimonial == null) return false;

            _context.Testimonials.Remove(testimonial);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}