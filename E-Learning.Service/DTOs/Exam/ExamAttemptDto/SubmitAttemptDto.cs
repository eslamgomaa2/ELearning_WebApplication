using System.ComponentModel.DataAnnotations;

public class SubmitAttemptDto
{
    [Required, MinLength(1)]
    public List<SubmitAnswerDto> Answers { get; set; } = new();
}