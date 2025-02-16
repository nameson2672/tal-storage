using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

public class FileRecord : BaseEntity
{
    public FileRecord(string name, string s3Url)
    {
        Name = name;
        S3Url = s3Url;
        FilesSharedWith = new List<FileShareRecord> {  };
    }
    [Required]
    public string Name { get; set; }

    public long Size { get; set; } = 0;

    public string MimeType { get; set; } = String.Empty;

    public string Status { get; set; } = String.Empty;

    [Required]
    public string S3Url { get; set; }

    public IEnumerable<FileShareRecord> FilesSharedWith { get; set; }
}