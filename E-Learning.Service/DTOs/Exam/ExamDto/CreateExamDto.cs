using System.ComponentModel.DataAnnotations;

public class CreateExamDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    public int CourseId { get; set; }

    public string? EducationLevel { get; set; }

    [Required]
    public DateTime ScheduledAt { get; set; }

    public DateTime? EndDateTime { get; set; }

    [Range(1, 7200)]
    public int DurationMinutes { get; set; }  // convert to seconds when mapping to entity

    public string? Instructions { get; set; }
    public string? Rules { get; set; }
    public string? TechnicalRequirements { get; set; }
    public decimal TotalMarks { get; set; }
    public decimal PassingScore { get; set; } = 60;
    public int MaxAttempts { get; set; } = 1;
    public bool AIShuffleEnabled { get; set; } = false;
}