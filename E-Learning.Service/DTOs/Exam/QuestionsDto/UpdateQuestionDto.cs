using System.ComponentModel.DataAnnotations;

public class UpdateQuestionDto
{
    [Required] public string QuestionText { get; set; } = string.Empty;
    [Required] public string QuestionType { get; set; } = string.Empty;
    public decimal Points { get; set; }
    public int Order { get; set; }
    public List<UpdateOptionDto> Options { get; set; } = new();
}