using Microsoft.AspNetCore.Mvc;

namespace StudentApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet]
    public async Task<IResult> HealthCheck()
    {
        var response = new
        {
            Message = "Api is healthy"
        };

        await Task.CompletedTask;

        return TypedResults.Ok(response);
    }
}
