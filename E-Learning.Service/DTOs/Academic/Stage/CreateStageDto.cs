namespace E_Learning.Service.DTOs.Academic.Stage
{
    public class CreateStageDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int OrderIndex { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
