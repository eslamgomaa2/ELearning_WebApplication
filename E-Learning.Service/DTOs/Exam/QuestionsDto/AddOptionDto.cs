using System.ComponentModel.DataAnnotations;

public class AddOptionDto
{
    [Required] public string OptionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}