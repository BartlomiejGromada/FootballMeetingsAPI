using Contracts.Models.FootballPitch;
using Domain.Repositories;
using FluentValidation;

namespace Contracts.Validators;

public class AddFootballPitchValidator : AbstractValidator<AddFootballPitchDto>
{
    public AddFootballPitchValidator(IRepositoryManager repositoryManager)
    {
        RuleFor(fp => fp.Name).NotEmpty().WithMessage("Football pitch name can't be null");
        RuleFor(fp => fp.Name).CustomAsync(async (value, context, cancellationToken) =>
        {
            var footballPitch = await repositoryManager.FootballPitchesRepository.GetByNameAsync(value, cancellationToken);
            if (footballPitch is not null)
            {
                context.AddFailure("Name", "Football pitch with this name is already exists");
            }
        }); 

        RuleFor(fp => fp.City).NotEmpty().WithMessage("City can't be null");
    }
}
