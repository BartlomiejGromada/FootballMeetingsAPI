using Contracts.Models.User;
using Contracts.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation.Controllers;
[ApiController]
[Route("api/account")]
public class UserController : ControllerBase
{
    private readonly IUsersService _userService;
	public UserController(IUsersService userService)
	{
		_userService = userService;
	}

	[HttpPost("register")]
	public async Task<ActionResult> RegisterUser([FromBody] RegisterUserDto dto, [FromServices] IValidator<RegisterUserDto> validator,
         CancellationToken cancellationToken = default)
	{
		var validationResult = await validator.ValidateAsync(dto, cancellationToken);
		if (!validationResult.IsValid)
		{
			validationResult.AddToModelState(this.ModelState);
			return BadRequest(this.ModelState);
		}

		var userId = await _userService.RegisterUserAsync(dto, cancellationToken);

		return Created($"api/account/{userId}", null);
	}
}
