using E_Learning.Core.Base;
using E_Learning.Service.DTOs.Academic.Level;

namespace E_Learning.Core.Interfaces.Services.Academic
{
    public interface ILevelService
    {
        Task<Response<LevelResponseDto>> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Response<IReadOnlyList<LevelResponseDto>>> GetAllAsync(CancellationToken ct = default);
        Task<Response<IReadOnlyList<LevelResponseDto>>> GetByStageIdAsync(int stageId, CancellationToken ct = default);
        Task<Response<LevelResponseDto>> CreateAsync(CreateLevelDto dto, CancellationToken ct = default);
        Task<Response<LevelResponseDto>> UpdateAsync(int id, UpdateLevelDto dto, CancellationToken ct = default);
        Task<Response<string>> DeleteAsync(int id, CancellationToken ct = default);
    }
}