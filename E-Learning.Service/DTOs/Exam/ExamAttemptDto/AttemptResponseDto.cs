public class AttemptResponseDto
{
    public int Id { get; set; }
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int ExamId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public decimal? Score { get; set; }
    public bool? IsPassed { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool IsPublished { get; set; }
    public string? TeacherComment { get; set; }
    public string? ReviewDecision { get; set; }
    public List<AttemptAnswerResponseDto> Answers { get; set; } = new();
}