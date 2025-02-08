using Amazon.S3;
using Amazon.S3.Model;

public class FileUploadService : IFileUploadService
{
    private readonly IAmazonS3 _s3Client;
    private readonly FileUploadDbContext _dbContext;

    private const string BucketName = "your-bucket-name";
    private const int UrlExpiryDurationMinutes = 30;

    public FileUploadService(IAmazonS3 s3Client, FileUploadDbContext dbContext)
    {
        _s3Client = s3Client;
        _dbContext = dbContext;
    }

    public async Task<(string PresignedUrl, string FileRecordId)> GeneratePresignedUrlAsync(string fileName, long size, string mimeType, string uploadedBy)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("FileName cannot be null or empty.", nameof(fileName));
        }

        var presignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = BucketName,
            Key = fileName,
            Expires = DateTime.UtcNow.AddMinutes(UrlExpiryDurationMinutes),
            Verb = HttpVerb.PUT
        };

        var url = _s3Client.GetPreSignedURL(presignedUrlRequest);

        var fileRecord = new FileRecord
        {
            Name = fileName,
            Size = size,
            MimeType = mimeType,
            Status = "Pending",
            S3Url = $"https://{BucketName}.s3.amazonaws.com/{fileName}"
        };

        await _dbContext.FileRecords.AddAsync(fileRecord);
        await _dbContext.SaveChangesAsync();

        return (url, fileRecord.Id);
    }
    public async Task<string> GetPresignedDownloadUrlAsync(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("FileName cannot be null or empty.", nameof(fileName));
        }

        var presignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = BucketName,
            Key = fileName,
            Expires = DateTime.UtcNow.AddMinutes(UrlExpiryDurationMinutes),
            Verb = HttpVerb.GET
        };

        // Generate the presigned URL for downloading
        var url = _s3Client.GetPreSignedURL(presignedUrlRequest);

        return url;
    }
}
