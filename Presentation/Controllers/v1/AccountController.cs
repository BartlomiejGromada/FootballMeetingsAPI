using Contracts.Models.Account;
using Contracts.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
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
    private readonly IUsersService _usersService;
    public AccountController(IAccountService accountService, IUsersService usersService)
    {
        _accountService = accountService;
        _usersService = usersService;
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

        var userId = await _accountService.RegisterUser(dto);

        return Created($"api/v1/account/{userId}", null);
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginUser([FromBody] LoginUserDto dto, [FromServices] IValidator<LoginUserDto> validator)
    {
        var validationResult = validator.Validate(dto);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(this.ModelState);
            return BadRequest(this.ModelState);
        }

        var jwtToken = await _accountService.GenerateJwt(dto);
        return Ok(jwtToken);
    }


    [HttpDelete("{accountId}")]
    [Authorize]
    public async Task<ActionResult> RemoveAccount([FromRoute] int accountId, [FromBody] PasswordDto dto)
    {        
        await _accountService.RemoveUserById(accountId, dto.Password);

        return NoContent();
    }

    [HttpPatch("{accountId}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> UpdateAccountPatch([FromRoute] int accountId, [FromBody] JsonPatchDocument user)
    {
        await _accountService.UpdateAccountPatch(accountId, user);

        return NoContent();
    }
}
