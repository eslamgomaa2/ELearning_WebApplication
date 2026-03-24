using System.ComponentModel.DataAnnotations;

public class SubmitAnswerDto
{
    [Required]
    public int QuestionId { get; set; }
    public int? SelectedOptionId { get; set; }   // MCQ / TrueFalse
    public string? TextAnswer { get; set; }       // open text questions
}