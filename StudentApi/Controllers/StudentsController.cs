using System.Net;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IResult> Retrieve(CancellationToken token = default)
    {
        var response = await studentService.GetAllStudents(token);

        if (response.StatusCode == HttpStatusCode.OK)
        {
            return TypedResults.Ok(response);
        }

        return TypedResults.InternalServerError(response.Message);
    }

    [HttpPost]
    public async Task<IResult> Add(IFormFile file, CancellationToken token = default)
    {
        if (file == null || file.Length == 0)
        {
            logger.LogWarning("File is missing");
            return TypedResults.BadRequest("File is empty");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (IsValidFileType(extension) is false)
        {
            var allowedTypes = $"[{string.Join(", ", ApiConstants.ValidFileTypes)}]";
            logger.LogWarning("Invalid file type. FileName: {FileName}", file.FileName);
            return TypedResults.BadRequest($"Invalid file type. Only {allowedTypes} files are accepted");
        }

        var response = await studentService.AddStudents(file, token);

        if (response.StatusCode == HttpStatusCode.Created)
        {
            return TypedResults.CreatedAtRoute(
                value: response,
                routeName: nameof(Retrieve),
                routeValues: token);
        }

        return TypedResults.InternalServerError(response.Message);
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
