using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentsController : ControllerBase
{
    [HttpGet]
    public async Task<IResult> Retrieve(CancellationToken token = default)
    {
        await Task.CompletedTask;

        return TypedResults.Ok();
    }

    [HttpPost]
    public async Task<IResult> Add(CancellationToken token = default)
    {
        await Task.CompletedTask;

        return TypedResults.Created();
    }
}
