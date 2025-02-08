using System.ComponentModel.DataAnnotations;
using TalStorage.Constants;

public class FileShareRecord : BaseEntity
{
    public FileShareRecord(Guid fileId, string sharedWith)
    {
        FileId = fileId;
        SharedWith = sharedWith;
    }

    [Required]
    public Guid FileId { get; set; }
    [Required]
    public string SharedWith { get; set; }
    public DateTime SharedAt { get; set; } = DateTime.UtcNow;
    public FileAccessAs FileAccessAs { get; set; } = FileAccessAs.Owner;
    public FileRecord File { get; set; }
}