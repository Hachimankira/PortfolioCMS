using PortfolioCMS.DTOs;
using PortfolioCMS.Models.DTOs;
using PortfolioCMS.Models.Wrappers;

namespace PortfolioCMS.Services.Interfaces
{
    public interface ITestimonialService
    {
        Task<PagedResult<TestimonialResponseDto>> GetAllAsync(string userId, PaginationFilter filter);
        Task<TestimonialResponseDto?> GetByIdAsync(int id, string userId);
        Task<TestimonialResponseDto> CreateAsync(CreateTestimonialDto dto, string userId);
        Task<TestimonialResponseDto?> UpdateAsync(UpdateTestimonialDto dto, string userId);
        Task<bool> DeleteAsync(int id, string userId);
    }
}