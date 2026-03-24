using System.ComponentModel.DataAnnotations;

public class UpdateAnswerScoreDto
{
    [Required]
    public int QuestionId { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Score { get; set; }

    public bool IsCorrect { get; set; }
}