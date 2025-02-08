using System.ComponentModel.DataAnnotations;

public class FileRecord : BaseEntity
{
    [Key]
    public string FileId { get; set; }

    [Required]
    public string Name { get; set; }

    public long Size { get; set; }

    public string MimeType { get; set; }

    public string Status { get; set; }

    [Required]
    public string S3Url { get; set; }

    public IEnumerable<FileShareRecord> FilesSharedWith { get; set; }
}