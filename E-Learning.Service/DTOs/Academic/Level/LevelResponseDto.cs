using E_Learning.Service.DTOs.Academic.Stage;

namespace E_Learning.Service.DTOs.Academic.Level
{
    public class LevelResponseDto
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        // Full stage data inside each level
        public StageSummaryDto Stage { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
