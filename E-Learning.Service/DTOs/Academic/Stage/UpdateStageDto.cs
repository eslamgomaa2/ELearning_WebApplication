namespace E_Learning.Service.DTOs.Academic.Stage
{
    public class UpdateStageDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? OrderIndex { get; set; }
        public bool? IsActive { get; set; }
    }
}
