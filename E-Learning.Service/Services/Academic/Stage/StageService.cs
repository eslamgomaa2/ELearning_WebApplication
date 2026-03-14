using AutoMapper;
using E_Learning.Core.Base;
using E_Learning.Core.Repository;
using E_Learning.Service.DTOs.Academic.Stage;
using E_Learning.Service.Services.Academic.Stage;

namespace E_Learning.Service.Services.Academic
{
    public class StageService : IStageService
    {
        private readonly IUnitOfWork _uow;
        private readonly ResponseHandler _responseHandler;
        private readonly IMapper _mapper;

        public StageService(IUnitOfWork uow, ResponseHandler responseHandler, IMapper mapper)
        {
            _uow = uow;
            _responseHandler = responseHandler;
            _mapper = mapper;
        }


        public async Task<Response<StageResponseDto>> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var stage = await _uow.Stages.GetByIdAsync(id, ct);

            if (stage is null)
                return _responseHandler.NotFound<StageResponseDto>(
                    $"Stage with ID {id} was not found.");

            return _responseHandler.Success(_mapper.Map<StageResponseDto>(stage));
        }


        public async Task<Response<IReadOnlyList<StageResponseDto>>> GetAllAsync(CancellationToken ct = default)
        {
            var stages = await _uow.Stages.GetAllAsync(ct);

            var result = _mapper.Map<IReadOnlyList<StageResponseDto>>(stages);
            return _responseHandler.Success(result);
        }


        public async Task<Response<StageResponseDto>> CreateAsync(
            CreateStageDto dto, CancellationToken ct = default)
        {
            bool nameExists = await _uow.Stages.ExistsAsync(dto.Name, ct);
            if (nameExists)
                return _responseHandler.BadRequest<StageResponseDto>(
                    $"A stage with the name '{dto.Name}' already exists.");

            var stage = new Core.Entities.Academic.Stage
            {
                Name = dto.Name,
                Description = dto.Description,
                OrderIndex = dto.OrderIndex,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            await _uow.Stages.AddAsync(stage, ct);
            await _uow.SaveChangesAsync(ct);

            var created = await _uow.Stages.GetByIdAsync(stage.Id, ct);
            return _responseHandler.Created(_mapper.Map<StageResponseDto>(created!));
        }


        public async Task<Response<StageResponseDto>> UpdateAsync(
            int id, UpdateStageDto dto, CancellationToken ct = default)
        {
            var stage = await _uow.Stages.GetByIdAsync(id, ct);

            if (stage is null)
                return _responseHandler.NotFound<StageResponseDto>(
                    $"Stage with ID {id} was not found.");

            if (dto.Name is not null && dto.Name != stage.Name)
            {
                bool nameExists = await _uow.Stages.ExistsAsync(dto.Name, ct);
                if (nameExists)
                    return _responseHandler.BadRequest<StageResponseDto>(
                        $"A stage with the name '{dto.Name}' already exists.");

                stage.Name = dto.Name;
            }

            if (dto.Description is not null)
                stage.Description = dto.Description;

            if (dto.OrderIndex.HasValue)
                stage.OrderIndex = dto.OrderIndex.Value;

            if (dto.IsActive.HasValue)
                stage.IsActive = dto.IsActive.Value;

            _uow.Stages.Update(stage);
            await _uow.SaveChangesAsync(ct);

            var updated = await _uow.Stages.GetByIdAsync(id, ct);
            return _responseHandler.Success(_mapper.Map<StageResponseDto>(updated!));
        }


        public async Task<Response<string>> DeleteAsync(int id, CancellationToken ct = default)
        {
            var stage = await _uow.Stages.GetByIdAsync(id, ct);

            if (stage is null)
                return _responseHandler.NotFound<string>(
                    $"Stage with ID {id} was not found.");

            if (stage.Levels.Any())
                return _responseHandler.BadRequest<string>(
                    "Cannot delete a stage that still has levels. Please delete or reassign the levels first.");

            _uow.Stages.Delete(stage);
            await _uow.SaveChangesAsync(ct);

            return _responseHandler.Deleted<string>();
        }
    }
}