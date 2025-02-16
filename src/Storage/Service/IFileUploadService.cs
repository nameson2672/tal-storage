public interface IFileUploadService
{
    Task<string> GeneratePresignedUrlAsync(string fileName, long size, string mimeType, string uploadedBy);
    Task<string> GetPresignedDownloadUrlAsync(string fileName);
}
