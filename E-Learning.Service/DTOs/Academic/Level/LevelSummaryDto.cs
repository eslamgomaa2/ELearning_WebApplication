namespace E_Learning.Service.DTOs.Academic.Level
{
    public class LevelSummaryDto
    {
        public int Id { get; set; }
        public int StageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
