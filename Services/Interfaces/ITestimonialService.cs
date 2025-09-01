using PortfolioCMS.Models.DTOs;

namespace PortfolioCMS.Services.Interfaces
{
    public interface ITestimonialService
    {
        Task<IEnumerable<TestimonialResponseDto>> GetAllAsync(string userId);
        Task<TestimonialResponseDto?> GetByIdAsync(int id, string userId);
        Task<TestimonialResponseDto> CreateAsync(CreateTestimonialDto dto, string userId);
        Task<TestimonialResponseDto?> UpdateAsync(UpdateTestimonialDto dto, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}