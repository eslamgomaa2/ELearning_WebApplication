using System.ComponentModel.DataAnnotations;

public class ReviewAttemptDto
{
    [Required]
    public string ReviewDecision { get; set; } = string.Empty;
    public string? TeacherComment { get; set; }
}