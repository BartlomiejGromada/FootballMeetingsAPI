namespace Domain.Exceptions;

public class MatchAlreadyTakenPlaceExpcetion : BadRequestException
{
    public MatchAlreadyTakenPlaceExpcetion(string message) : base(message)
    {
    }
}
