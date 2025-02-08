using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalStorage.Controllers;
using TalStorage.Service;

public class FileUploadController : BaseController
{
    private readonly IFileUploadService _fileUploadService;
    private readonly FileUploadDbContext _dbContext;
    private readonly IUserService _userService;

    public FileUploadController(IFileUploadService fileUploadService, FileUploadDbContext dbContext, IUserService userService)
    {
        _fileUploadService = fileUploadService;
        _dbContext = dbContext;
        _userService = userService;
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
            //var (presignedUrl, fileRecordId) = await _fileUploadService.GeneratePresignedUrlAsync(model.FileName, model.Size, model.MimeType, model.UploadedBy);/var
            var (presignedUrl, fileRecordId) = ("you got it", "file record id"); 
            var fileRecorcd = new FileRecord(model.FileName, model.FileName);
            fileRecorcd.AddSharedUsers(new FileShareRecord(fileRecorcd.Id, _userService.GetUserId()));
            await _dbContext.SaveChangesAsync();
            return Ok(new { PresignedUrl = presignedUrl, FileRecordId = fileRecordId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
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
}