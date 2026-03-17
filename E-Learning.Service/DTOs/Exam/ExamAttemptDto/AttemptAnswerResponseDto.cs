public class AttemptAnswerResponseDto
{
    public int AttemptId { get; set; }
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public int? SelectedOptionId { get; set; }
    public string? SelectedOptionText { get; set; }
    public string? TextAnswer { get; set; }
    public bool? IsCorrect { get; set; }
    public decimal? Score { get; set; }
}