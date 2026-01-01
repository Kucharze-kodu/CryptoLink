using Microsoft.AspNetCore.Mvc;
using MediatR;
using CryptoLink.Application.Features.Users.RegisterInit;
using CryptoLink.Application.Features.Users.Register;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register/init")]
    public async Task<ActionResult<string>> InitiateRegister([FromBody] RegisterInitCommand command)
    {
        var encryptedMessage = await _mediator.Send(command);
        return Ok(encryptedMessage);
    }

    [HttpPost("register/complete")]
    public async Task<ActionResult<Guid>> CompleteRegister([FromBody] RegisterCommand command)
    {
        var userId = await _mediator.Send(command);
        return Ok(userId);
    }
}