using TalStorage.Constants;

namespace TalStorage.DTOs
{
    public class ShareFileRequest
    {
        public Guid FileId { get; set; }
        public string SharedWith { get; set; }
        public FileAccessAs FileAccessAs { get; set; }  // Viewer, Editor, Owner
    }
}
