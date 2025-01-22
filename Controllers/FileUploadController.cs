using Microsoft.AspNetCore.Mvc;
using TalStorage.Controllers;

public class FileUploadController : BaseController
{
    private readonly IFileUploadService _fileUploadService;

    public FileUploadController(IFileUploadService fileUploadService)
    {
        _fileUploadService = fileUploadService;
    }

    [HttpPost]
    public async Task<IActionResult> GeneratePresignedUrl([FromBody] GeneratePresignedUrlViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var (presignedUrl, fileRecordId) = await _fileUploadService.GeneratePresignedUrlAsync(model.FileName, model.Size, model.MimeType, model.UploadedBy);
            return Ok(new { PresignedUrl = presignedUrl, FileRecordId = fileRecordId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}