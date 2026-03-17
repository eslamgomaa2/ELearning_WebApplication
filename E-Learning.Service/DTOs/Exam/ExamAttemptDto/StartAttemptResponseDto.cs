public class StartAttemptResponseDto
{
    public int AttemptId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime ExamEndsAt { get; set; }  
    public int TotalQuestions { get; set; }
    public List<QuestionForStudentDto> Questions { get; set; } = new();
}