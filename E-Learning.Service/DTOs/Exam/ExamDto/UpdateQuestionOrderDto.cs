using System.ComponentModel.DataAnnotations;

public class UpdateQuestionOrderDto
{
    [Required]
    public bool AIShuffleEnabled { get; set; }
}