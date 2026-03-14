using E_Learning.Service.DTOs.Academic.Stage;
using E_Learning.Service.Services.Academic.Stage;
using Microsoft.AspNetCore.Mvc;

namespace E_Learning.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StagesController : ControllerBase
    {
        private readonly IStageService _stageService;

        public StagesController(IStageService stageService)
        {
            _stageService = stageService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var response = await _stageService.GetAllAsync(ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var response = await _stageService.GetByIdAsync(id, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStageDto dto, CancellationToken ct)
        {
            var response = await _stageService.CreateAsync(dto, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateStageDto dto, CancellationToken ct)
        {
            var response = await _stageService.UpdateAsync(id, dto, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var response = await _stageService.DeleteAsync(id, ct);
            return StatusCode((int)response.HttpStatusCode, response);
        }
    }
}