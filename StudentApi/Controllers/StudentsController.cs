using System.Net;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Common;
using StudentApi.Constants;
using StudentApi.Services.Interfaces;

namespace StudentApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(
    IStudentService studentService,
    ILogger<StudentsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Retrieve(
        [FromQuery] PaginationParameters parameters,
        CancellationToken token = default)
    {
        var response = await studentService.GetAllStudents(
            parameters: parameters,
            token: token);

        return StatusCode(statusCode: (int)response.StatusCode, value: response);
    }

    [HttpPost]
    public async Task<IActionResult> Import(IFormFile file, CancellationToken token = default)
    {
        if (file == null || file.Length == 0)
        {
            logger.LogWarning("File is missing");
            return BadRequest(new ServiceResult(
                statusCode: HttpStatusCode.BadRequest,
                message: "File is empty"));
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (IsValidFileType(extension) is false)
        {
            var allowedTypes = $"[{string.Join(", ", ApiConstants.ValidFileTypes)}]";
            logger.LogWarning("Invalid file type. FileName: {FileName}", file.FileName);
            return BadRequest(new ServiceResult(
                statusCode: HttpStatusCode.BadRequest,
                message: $"Invalid file type. Only {allowedTypes} files are accepted"));
        }

        var response = await studentService.ImportStudents(file, token);

        return StatusCode(statusCode: (int)response.StatusCode, value: response);
    }

    private static bool IsValidFileType(string type)
    {
        if (ApiConstants.ValidFileTypes.Contains(type))
        {
            return true;
        }

        return false;
    }
}
