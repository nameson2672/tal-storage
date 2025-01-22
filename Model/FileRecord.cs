using System.ComponentModel.DataAnnotations;

public class FileRecord : BaseEntity
{
    [Key]
    public int FileId { get; set; }

    [Required]
    public string Name { get; set; }

    public long Size { get; set; }

    [Required]
    public string MimeType { get; set; }

    [Required]
    public string UploadedBy { get; set; }

    [Required]
    public string Status { get; set; }

    [Required]
    public string S3Url { get; set; }

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}