using System.ComponentModel.DataAnnotations;

public class AddQuestionItemDto
{
    [Required] public string QuestionText { get; set; } = string.Empty;
    [Required] public string QuestionType { get; set; } = string.Empty; // MCQ | TrueFalse | Text
    public decimal Points { get; set; } = 1;
    public int Order { get; set; }
    public List<AddOptionDto> Options { get; set; } = new();
}