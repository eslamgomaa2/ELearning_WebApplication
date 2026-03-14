using E_Learning.Core.Base;

namespace E_Learning.Core.Entities.Academic
{
    public class Level : BaseEntity
    {
        public int StageId { get; set; }
        public Stage Stage { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public int OrderIndex { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
