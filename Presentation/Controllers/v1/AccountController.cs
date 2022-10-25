using Contracts.Models.Account;
using Contracts.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers.v1;
[ApiController]
[Route("api/v1/account")]
[Produces("application/json")]
[ApiVersion("1.0")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;
    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto dto, [FromServices] IValidator<RegisterUserDto> validator,
         CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var userId = await _accountService.RegisterUserAsync(dto, cancellationToken);

        return Created($"api/v1/account/{userId}", null);
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUser([FromBody] LoginUserDto dto, [FromServices] IValidator<LoginUserDto> validator,
        CancellationToken cancellationToken = default)
    {
        var validationResult = validator.Validate(dto);
        if(!validationResult.IsValid)
        {
            validationResult.AddToModelState(this.ModelState);
            return BadRequest(this.ModelState);
        }

        var jwtToken = await _accountService.GenerateJwtAsync(dto, cancellationToken);
        return Ok(jwtToken);
    }

}
