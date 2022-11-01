namespace Domain.Exceptions;

public class FootballMatchHasNotBeenPlayedYetException : BadRequestException
{
    public FootballMatchHasNotBeenPlayedYetException(string message) : base(message)
    {
    }
}
