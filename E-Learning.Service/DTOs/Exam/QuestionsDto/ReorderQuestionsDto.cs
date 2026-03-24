using System.ComponentModel.DataAnnotations;

public class ReorderQuestionsDto
{
    [Required, MinLength(1)]
    public List<int> QuestionIdsInOrder { get; set; } = new();
}