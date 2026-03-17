public class OptionForStudentDto
{
    public int Id { get; set; }
    public string OptionText { get; set; } = string.Empty;
    // ❌ No IsCorrect — never expose to student
}