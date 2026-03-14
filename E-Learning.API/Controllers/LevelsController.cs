using E_Learning.Core.Interfaces.Services.Academic;
using E_Learning.Service.DTOs.Academic.Level;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LevelsController : ControllerBase
    {
        private readonly ILevelService _levelService;

        public LevelsController(ILevelService levelService)
        {
            _levelService = levelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var response = await _levelService.GetAllAsync(ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var response = await _levelService.GetByIdAsync(id, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("SearchByStage/{stageId:int}")]
        public async Task<IActionResult> GetByStage(int stageId, CancellationToken ct)
        {
            var response = await _levelService.GetByStageIdAsync(stageId, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLevelDto dto, CancellationToken ct)
        {
            var response = await _levelService.CreateAsync(dto, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLevelDto dto, CancellationToken ct)
        {
            var response = await _levelService.UpdateAsync(id, dto, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _levelService.DeleteAsync(id, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}