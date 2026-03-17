public class QuestionResponseDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public decimal Points { get; set; }
    public int Order { get; set; }
    public bool IsAIGenerated { get; set; }
    public List<OptionResponseDto> Options { get; set; } = new();
}