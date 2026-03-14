using E_Learning.Service.DTOs.Academic.Level;

namespace E_Learning.Service.DTOs.Academic.Stage
{
    public class StageResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        // Full levels list inside each stage
        public IReadOnlyList<LevelSummaryDto> Levels { get; set; }
            = new List<LevelSummaryDto>();
    }
}
