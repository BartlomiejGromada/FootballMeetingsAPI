namespace Domain.Exceptions;

public class MaximumNumberOfPlayersExceededException : BadRequestException
{
    public MaximumNumberOfPlayersExceededException(string message) : base(message)
    {
    }
}
