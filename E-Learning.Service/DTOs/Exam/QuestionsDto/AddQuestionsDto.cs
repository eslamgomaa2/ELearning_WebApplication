using System.ComponentModel.DataAnnotations;

public class AddQuestionsDto
{
    [Required, MinLength(1)]
    public List<AddQuestionItemDto> Questions { get; set; } = new();
}