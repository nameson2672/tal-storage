using Amazon.S3;
using Amazon.S3.Model;

public class FileUploadService : IFileUploadService
{
    private readonly IAmazonS3 _s3Client;
    private readonly FileUploadDbContext _dbContext;
    private const int UrlExpiryDurationMinutes = 30;
    private readonly IConfiguration _configuration;

    public FileUploadService(IAmazonS3 s3Client, FileUploadDbContext dbContext, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task<string> GeneratePresignedUrlAsync(string fileName, long size, string mimeType, string uploadedBy)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("FileName cannot be null or empty.", nameof(fileName));
        }

        var presignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _configuration["AWS:BucketName"],
            Key = fileName,
            Expires = DateTime.UtcNow.AddMinutes(UrlExpiryDurationMinutes),
            Verb = HttpVerb.PUT
        };

        var url = _s3Client.GetPreSignedURL(presignedUrlRequest);

        return url;
    }
    public async Task<string> GetPresignedDownloadUrlAsync(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            throw new ArgumentException("FileName cannot be null or empty.", nameof(fileName));
        }

        var presignedUrlRequest = new GetPreSignedUrlRequest
        {
            BucketName = _configuration["AWS:BucketName"],
            Key = fileName,
            Expires = DateTime.UtcNow.AddMinutes(UrlExpiryDurationMinutes),
            Verb = HttpVerb.GET
        };

        // Generate the presigned URL for downloading
        var url = _s3Client.GetPreSignedURL(presignedUrlRequest);

        return url;
    }
}
