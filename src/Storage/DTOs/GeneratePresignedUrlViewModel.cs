using System.ComponentModel.DataAnnotations;

public class GeneratePresignedUrlViewModel
{
    [Required]
    public string FileName { get; set; }

    [Required]
    public long Size { get; set; }

    [Required]
    public string MimeType { get; set; }

    [Required]
    public string UploadedBy { get; set; }
}
