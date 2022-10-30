using Contracts.Models.Account;
using Domain.Repositories;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Services.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
	public RegisterUserValidator(IRepositoryManager repositoryManager)
	{

		RuleFor(u => u.Email).EmailAddress().WithMessage("Invalid format email");
		RuleFor(u => u.Email).CustomAsync(async (value, context, cancellationToken) =>
		{
			if (!string.IsNullOrEmpty(value))
			{
				var user = await repositoryManager.UsersRepository.GetUserByEmailAsync(value, cancellationToken);
				if (user != null)
				{
					context.AddFailure("Email", "That email is taken");
				}
			}
		});

		RuleFor(u => u.ConfirmPassword).Equal(u => u.Password).WithMessage("Confirm password must be the same like password");
		RuleFor(u => u.NickName).NotEmpty().WithMessage("Nickname can't be null");

		RuleFor(u => u.Password).Custom((value, context) =>
		{
			if(string.IsNullOrEmpty(value))
			{
				context.AddFailure("Password", "Password can't be null");
			}

			if(value.Length < 6)
			{
				context.AddFailure("Password", "Password must be at least 6 characters long");
			}

			if(!(new Regex("[A-Z]+").Match(value).Success))
			{
				context.AddFailure("Password", "Password must have at least one upper case");
			}

			if(!(new Regex("[0-9]+").Match(value).Success))
			{
                context.AddFailure("Password", "Password must have at least one digit");
            }
		});
	}
}
