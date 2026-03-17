public class QuestionForStudentDto
{
    public int Id { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public string QuestionType { get; set; } = string.Empty;
    public decimal Points { get; set; }
    public List<OptionForStudentDto> Options { get; set; } = new();
}