using Contracts.Models.Account;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Contracts.Validators;

public class LoginUserValidator : AbstractValidator<LoginUserDto>
{
	public LoginUserValidator()
	{
		RuleFor(u => u.Email).EmailAddress().WithMessage("Invalid format email");

        RuleFor(u => u.Password).Custom((value, context) =>
        {
            if (string.IsNullOrEmpty(value))
            {
                context.AddFailure("Password", "Password can't be null");
            }

            if (value.Length < 6)
            {
                context.AddFailure("Password", "Password must be at least 6 characters long");
            }

            if (!(new Regex("[A-Z]+").Match(value).Success))
            {
                context.AddFailure("Password", "Password must have at least one upper case");
            }

            if (!(new Regex("[0-9]+").Match(value).Success))
            {
                context.AddFailure("Password", "Password must have at least one digit");
            }
        });
    }
}
