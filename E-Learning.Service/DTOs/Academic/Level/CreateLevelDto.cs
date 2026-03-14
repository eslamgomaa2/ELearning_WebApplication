namespace E_Learning.Service.DTOs.Academic.Level
{
    public class CreateLevelDto
    {
        public int StageId { get; set; }
        public string Name { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
