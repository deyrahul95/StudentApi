using System.Net;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Common;
using StudentApi.Constants;
using StudentApi.Models;
using StudentApi.Services.Interfaces;

namespace StudentApi.Controllers;

/// <summary>
/// API controller for managing student-related operations
/// Routes are prefixed with "api/students"
/// Inherits from <see cref="ControllerBase"/>
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class StudentsController(
    IStudentService studentService,
    ILogger<StudentsController> logger) : ControllerBase
{
    /// <summary>
    /// Retrieves a paged list of students based on query parameters
    /// </summary>
    /// <param name="parameters">Parameters for filtering, sorting, and pagination</param>
    /// <param name="token">A cancellation token to observe while waiting for the task to complete</param>
    /// <returns>An <see cref="IActionResult"/> containing the paged list of students</returns>
    [HttpGet]
    public async Task<IActionResult> Retrieve(
        [FromQuery] StudentQueryParameters parameters,
        CancellationToken token = default)
    {
        var response = await studentService.GetAllStudents(
            parameters: parameters,
            token: token);

        return StatusCode(statusCode: (int)response.StatusCode, value: response);
    }

    /// <summary>
    /// Imports student data from the uploaded file
    /// </summary>
    /// <param name="file">The file containing student data to import</param>
    /// <param name="token">A cancellation token to observe while waiting for the task to complete</param>
    /// <returns>An <see cref="IActionResult"/> indicating the result of the import operation</returns>
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

    /// <summary>
    /// Determines whether the specified file type is valid for import
    /// </summary>
    /// <param name="type">The MIME type of the file to validate</param>
    /// <returns><c>true</c> if the file type is valid; otherwise, <c>false</c></returns>
    private static bool IsValidFileType(string type)
    {
        if (ApiConstants.ValidFileTypes.Contains(type))
        {
            return true;
        }

        return false;
    }
}
