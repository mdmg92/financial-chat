using FinancialChat.WebApi.Domain.Commands;
using FinancialChat.WebApi.Domain.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FinancialChat.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IMediator mediator, ILogger<ChatController> logger)
    {
        _mediator = mediator;
        _logger   = logger;
    }

    [HttpPost("messages")]
    public async Task<IActionResult> Post([FromBody] NewMessage message)
    {
        await _mediator.Send(message);

        return Ok();
    }

    [HttpGet("messages")]
    public async Task<IActionResult> Get()
        => Ok(await _mediator.Send(new GetMessages()));
}