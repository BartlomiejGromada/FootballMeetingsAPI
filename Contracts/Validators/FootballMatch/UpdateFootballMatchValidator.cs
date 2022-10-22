using Contracts.Models.FootballMatch;
using Domain.Repositories;
using FluentValidation;

namespace Contracts.Validators.FootballMatch;

public class UpdateFootballMatchValidator : AbstractValidator<UpdateFootballMatchDto>
{
	public UpdateFootballMatchValidator(IRepositoryManager repositoryManager)
	{
        RuleFor(fm => fm.Name).NotEmpty().WithMessage("Football match name can't be null");
        RuleFor(fm => fm.MaxNumberOfPlayers).Custom((value, context) =>
        {
            if (value != null && value <= 1)
            {
                context.AddFailure("MaxNumberOfPlayers", "Max number of players must be null or greater than 1");
            }
        });

        RuleFor(fm => fm.Date).Custom((value, context) =>
        {
            if (value.Date < DateTime.UtcNow.Date)
            {
                context.AddFailure("Date", "Date of football match can't be past date");
            }
        });

        RuleFor(fm => fm.FootballPitchId).NotNull().WithMessage("Football pitch can't be null");
        RuleFor(fm => fm.FootballPitchId).CustomAsync(async (value, context, cancellationToken) =>
        {
            var existsFootballPitch = await repositoryManager.FootballPitchesRepository.ExistsByIdAsync(value, cancellationToken);
            if (!existsFootballPitch)
            {
                context.AddFailure("FootballPitch", "Football pitch doesn't exist");
            }
        });

        RuleFor(fm => fm.PlayersIds).CustomAsync(async (playersIds, context, cancellationToken) =>
        {
            var invalidPlayersIds = new List<int>();
            foreach (var playerId in playersIds)
            {
                var existsUser = await repositoryManager.UsersRepository.ExistsByIdAsync(playerId, cancellationToken);
                if (!existsUser)
                {
                    invalidPlayersIds.Add(playerId);
                }
            }
            if (invalidPlayersIds.Any())
            {
                context.AddFailure("Players", $"Invalid players ids: {string.Join(",", invalidPlayersIds)}");
            }
        });
    }
}