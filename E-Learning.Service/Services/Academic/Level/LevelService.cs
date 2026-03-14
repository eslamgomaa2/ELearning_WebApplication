using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Entities.Academic;
using E_Learning.Core.Interfaces.Services.Academic;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.Academic.Level;

namespace E_Learning.Service.Services.Academic
{
    public class LevelService : ILevelService
    {
        private readonly IUnitOfWork _uow;
        private readonly ResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public LevelService(IUnitOfWork uow, ResponseHandler responseHandler, IMapper mapper)
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }


        public async Task<Response<LevelResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var level = await _uow.Levels.GetByIdAsync(id, ct);

            if (level is null)
                return _responseHandler.NotFound<LevelResponseDto>(
                    $"Level with ID {id} was not found.");

            return _responseHandler.Success(_mapper.Map<LevelResponseDto>(level));
        }


        public async Task<Response<IReadOnlyList<LevelResponseDto>>> GetAllAsync(CancellationToken ct = default)
        {
            var levels = await _uow.Levels.GetAllAsync(ct);

            var result = _mapper.Map<IReadOnlyList<LevelResponseDto>>(levels);
            return _responseHandler.Success(result);
        }


        public async Task<Response<IReadOnlyList<LevelResponseDto>>> GetByStageIdAsync(
            int stageId, CancellationToken ct = default)
        {
            bool stageExists = await _uow.Stages.ExistsAsync(stageId, ct);
            if (!stageExists)
                return _responseHandler.NotFound<IReadOnlyList<LevelResponseDto>>(
                    $"Stage with ID {stageId} was not found.");

            var levels = await _uow.Levels.GetByStageIdAsync(stageId, ct);

            var result = _mapper.Map<IReadOnlyList<LevelResponseDto>>(levels);
            return _responseHandler.Success(result);
        }


        public async Task<Response<LevelResponseDto>> CreateAsync(
            CreateLevelDto dto, CancellationToken ct = default)
        {
            bool stageExists = await _uow.Stages.ExistsAsync(dto.StageId, ct);
            if (!stageExists)
                return _responseHandler.NotFound<LevelResponseDto>(
                    $"Stage with ID {dto.StageId} was not found.");

            bool nameExists = await _uow.Levels.ExistsAsync(dto.Name, dto.StageId, ct);
            if (nameExists)
                return _responseHandler.BadRequest<LevelResponseDto>(
                    $"A level with the name '{dto.Name}' already exists in this stage.");

            var level = new Level
            {
                StageId = dto.StageId,
                Name = dto.Name,
                OrderIndex = dto.OrderIndex,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Levels.AddAsync(level, ct);
            await _uow.SaveChangesAsync(ct);

            var created = await _uow.Levels.GetByIdAsync(level.Id, ct);
            return _responseHandler.Created(_mapper.Map<LevelResponseDto>(created!));
        }


        public async Task<Response<LevelResponseDto>> UpdateAsync(
            int id, UpdateLevelDto dto, CancellationToken ct = default)
        {
            var level = await _uow.Levels.GetByIdAsync(id, ct);

            if (level is null)
                return _responseHandler.NotFound<LevelResponseDto>(
                    $"Level with ID {id} was not found.");

            if (dto.Name is not null && dto.Name != level.Name)
            {
                bool nameExists = await _uow.Levels.ExistsAsync(dto.Name, level.StageId, ct);
                if (nameExists)
                    return _responseHandler.BadRequest<LevelResponseDto>(
                        $"A level with the name '{dto.Name}' already exists in this stage.");

                level.Name = dto.Name;
            }

            if (dto.OrderIndex.HasValue)
                level.OrderIndex = dto.OrderIndex.Value;

            if (dto.IsActive.HasValue)
                level.IsActive = dto.IsActive.Value;

            _uow.Levels.Update(level);
            await _uow.SaveChangesAsync(ct);

            var updated = await _uow.Levels.GetByIdAsync(id, ct);
            return _responseHandler.Success(_mapper.Map<LevelResponseDto>(updated!));
        }


        public async Task<Response<string>> DeleteAsync(int id, CancellationToken ct = default)
        {
            var level = await _uow.Levels.GetByIdAsync(id, ct);

            if (level is null)
                return _responseHandler.NotFound<string>(
                    $"Level with ID {id} was not found.");

            _uow.Levels.Delete(level);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Deleted<string>();
        }
    }
}