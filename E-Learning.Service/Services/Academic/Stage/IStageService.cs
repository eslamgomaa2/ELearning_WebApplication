using E_Learning.Core.Base;
using E_Learning.Service.DTOs.Academic.Stage;

namespace E_Learning.Service.Services.Academic.Stage
{
    public interface IStageService
    {
        Task<Response<StageResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Response<IReadOnlyList<StageResponseDto>>> GetAllAsync(CancellationToken ct = default);
        Task<Response<StageResponseDto>> CreateAsync(CreateStageDto dto, CancellationToken ct = default);
        Task<Response<StageResponseDto>> UpdateAsync(int id, UpdateStageDto dto, CancellationToken ct = default);
        Task<Response<string>> DeleteAsync(int id, CancellationToken ct = default);
    }
}