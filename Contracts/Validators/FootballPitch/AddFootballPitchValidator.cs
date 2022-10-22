using Contracts.Models.FootballPitch;
using Domain.Repositories;
using FluentValidation;

namespace Contracts.Validators.FootballPitch;

public class AddFootballPitchValidator : AbstractValidator<AddFootballPitchDto>
{
    public AddFootballPitchValidator(IRepositoryManager repositoryManager)
    {
        RuleFor(fp => fp.Name).NotEmpty().WithMessage("Football pitch name can't be null");
        RuleFor(fp => fp.Name).CustomAsync(async (value, context, cancellationToken) =>
        {
            var existFootballPitch = await repositoryManager.FootballPitchesRepository.ExistsByNameAsync(value, cancellationToken);
            if (existFootballPitch)
            {
                context.AddFailure("Name", "Football pitch with this name is already exists");
            }
        });

        RuleFor(fp => fp.City).NotEmpty().WithMessage("City can't be null");
    }
}
