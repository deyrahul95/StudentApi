using System.Net;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Services;

namespace StudentApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController(IStudentService studentService) : ControllerBase
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
    public async Task<IResult> Add(CancellationToken token = default)
    {
        var response = await studentService.AddStudents(token);

        if (response.StatusCode == HttpStatusCode.Created)
        {
            return TypedResults.CreatedAtRoute(
                value: response,
                routeName: nameof(Retrieve),
                routeValues: token);
        }

        return TypedResults.InternalServerError(response.Message);
    }
}
