using Contracts.Models.FootballPitch;
using FluentValidation;

namespace Contracts.Validators.FootballPitch;

public class UpdateFootballPitchValidator : AbstractValidator<UpdateFootballPitchDto>
{
	public UpdateFootballPitchValidator()
	{
        RuleFor(fp => fp.Name).NotEmpty().WithMessage("Football pitch name can't be null");

        RuleFor(fp => fp.City).NotEmpty().WithMessage("City can't be null");
    }
}
