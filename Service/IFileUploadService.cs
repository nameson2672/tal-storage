public interface IFileUploadService
{
    Task<(string PresignedUrl, int FileRecordId)> GeneratePresignedUrlAsync(string fileName, long size, string mimeType, string uploadedBy);
    Task<string> GetPresignedDownloadUrlAsync(string fileName);
}
