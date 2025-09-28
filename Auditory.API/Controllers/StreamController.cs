using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Auditory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StreamController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

    [HttpGet("user/{userName}")]
    public async Task<IActionResult> GetStreamsByUser(string userName)
    {
        try
        {
            var query = new Application.Queries.GetStreamsByUserQuery(userName);
            var streams = await _mediator.Send(query);
            return Ok(streams);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}