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

    [HttpGet("{streamId:guid}")]
    public async Task<IActionResult> GetStreamById(Guid streamId)
    {
        try
        {
            if (streamId == Guid.Empty)
                return BadRequest("Stream ID cannot be empty");

            var query = new Application.Queries.GetStreamByIdQuery(streamId);
            var stream = await _mediator.Send(query);
            if (stream is null)
                return NotFound("Stream not found");

            return Ok(stream);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateStream([FromBody] Application.Commands.CreateStreamCommand command)
    {
        try
        {
            if (command is null)
                return BadRequest("Command cannot be null");
            
            var createdStream = await _mediator.Send(command);
            if(createdStream is null)
                return BadRequest("Stream could not be created");
            
            return CreatedAtAction(nameof(GetStreamById), new { streamId = createdStream.Id, createdStream });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}