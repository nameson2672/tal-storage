using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TalStorage.Constants;
using TalStorage.Controllers;
using TalStorage.DTOs;
using TalStorage.Models;
using TalStorage.Service;

public class FileUploadController : BaseController
{
    private readonly IFileUploadService _fileUploadService;
    private readonly FileUploadDbContext _dbContext;
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    public FileUploadController(IFileUploadService fileUploadService, FileUploadDbContext dbContext, IUserService userService, UserManager<ApplicationUser> userManager)
    {
        _fileUploadService = fileUploadService;
        _dbContext = dbContext;
        _userService = userService;
        _userManager = userManager;
    }
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> GeneratePresignedUrl([FromBody] GeneratePresignedUrlViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var presignedUrl = await _fileUploadService.GeneratePresignedUrlAsync(model.FileName, model.Size, model.MimeType, model.UploadedBy);
            //var (presignedUrl, fileRecordId) = ("you got it", "file record id"); 
            var fileRecord = new FileRecord(model.FileName, model.FileName, model.MimeType, model.Size);
            var fileShareRecord = new FileShareRecord(fileRecord.Id, _userService.GetUserId());
            fileRecord.FilesSharedWith = new List<FileShareRecord> { fileShareRecord };
            _dbContext.FileRecords.Add(fileRecord);
            await _dbContext.SaveChangesAsync();
            return Ok(new { PresignedUrl = presignedUrl, FileRecordId = fileRecord.Id });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpGet("generate-presigned-url")]
    public async Task<IActionResult> GeneratePresignedUrlToGetFile([FromQuery] string fileName)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var presignedUrl = await _fileUploadService.GetPresignedDownloadUrlAsync(fileName);
            return Ok(new { PresignedUrl = presignedUrl});
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet("get-my-files")]
    [Authorize]
    public async Task<IActionResult> GetUserFiles()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found.");
        }
        var files = await _dbContext.FileRecords
            .Include(f => f.FilesSharedWith)
            .Where(f => f.FilesSharedWith.Any(fs => fs.SharedWith == userId)) // Fetch files where user is shared with
            .Select(f => new
            {
                f.Id,
                f.Name,
                f.S3Url,
                f.Size,
                f.MimeType,
                f.Status,
                SharedWith = f.FilesSharedWith.Select(fs => new
                {
                    fs.SharedWith,
                    fs.FileAccessAs,
                    fs.SharedAt,
                    User = _dbContext.Users
                        .Where(u => u.Id == fs.SharedWith)
                        .Select(u => new
                        {
                            u.FullName,
                            u.Email
                        })
                        .FirstOrDefault()
                })
            })
            .ToListAsync();


        return Ok(files);
    }

    
    [Authorize]
    [HttpPost("share")]
    public async Task<IActionResult> ShareFile([FromBody] ShareFileRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found.");
        }

        var file = await _dbContext.FileRecords
            .Include(f => f.FilesSharedWith)
            .FirstOrDefaultAsync(f => f.Id == request.FileId);

        if (file == null)
        {
            return NotFound("File not found.");
        }

        // Ensure the logged-in user is the owner of the file
        var ownershipRecord = file.FilesSharedWith.FirstOrDefault(fs => fs.SharedWith == userId && fs.FileAccessAs == FileAccessAs.Owner);
        if (ownershipRecord == null)
        {
            return Forbid("Only the owner can share this file.");
        }

        // Check if the target user exists
        var targetUser = await _userManager.FindByIdAsync(request.SharedWith);
        if (targetUser == null)
        {
            return NotFound("User to share with not found.");
        }

        // Prevent duplicate sharing
        var existingShare = file.FilesSharedWith.FirstOrDefault(fs => fs.SharedWith == request.SharedWith);
        if (existingShare != null)
        {
            return Conflict("File is already shared with this user.");
        }

        // Create a new sharing record
        var fileShareRecord = new FileShareRecord(request.FileId, request.SharedWith, request.FileAccessAs);

        _dbContext.FileShareRecords.Add(fileShareRecord);
        await _dbContext.SaveChangesAsync();

        return Ok("File shared successfully.");
    }
}