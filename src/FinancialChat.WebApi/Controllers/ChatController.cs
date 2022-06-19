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

    [HttpGet("messages")]
    public async Task<IActionResult> Get()
        => Ok(await _mediator.Send(new GetMessages()));
}