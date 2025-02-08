public interface IFileUploadService
{
    Task<(string PresignedUrl, string FileRecordId)> GeneratePresignedUrlAsync(string fileName, long size, string mimeType, string uploadedBy);
    Task<string> GetPresignedDownloadUrlAsync(string fileName);
}
