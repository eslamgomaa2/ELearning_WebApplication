using System.ComponentModel.DataAnnotations;

public class UpdateOptionDto
{
    public int? Id { get; set; }   // null = new option
    [Required] public string OptionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}