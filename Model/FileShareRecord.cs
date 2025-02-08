using System.ComponentModel.DataAnnotations;
using TalStorage.Constants;

public class FileShareRecord : BaseEntity
{
    [Key]
    public string Id { get; set; }
    [Required]
    public Guid FileId { get; set; }
    [Required]
    public Guid SharedWith { get; set; }
    public DateTime SharedAt { get; set; } = DateTime.UtcNow;
    public FileAccessAs FileAccessAs { get; set; } = FileAccessAs.Owner;
    public FileRecord File { get; set; }
}