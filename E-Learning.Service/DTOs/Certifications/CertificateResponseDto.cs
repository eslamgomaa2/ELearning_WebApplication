public class CertificateResponseDto
{
    public int Id { get; set; }
    public Guid StudentId { get; set; }
    public int CourseId { get; set; }
    public string CertificateCode { get; set; } = string.Empty;
    public DateTime IssuedAt { get; set; }
    public string? FileUrl { get; set; }
}