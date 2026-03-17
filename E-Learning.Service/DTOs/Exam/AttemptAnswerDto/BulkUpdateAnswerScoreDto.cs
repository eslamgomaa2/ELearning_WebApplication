using System.ComponentModel.DataAnnotations;

public class BulkUpdateAnswerScoreDto
{
    [Required, MinLength(1)]
    public List<UpdateAnswerScoreDto> Answers { get; set; } = new();
}