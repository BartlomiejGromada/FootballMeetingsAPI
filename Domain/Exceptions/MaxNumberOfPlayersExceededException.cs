namespace Domain.Exceptions;

public class MaxNumberOfPlayersExceededException : BadRequestException
{
    public MaxNumberOfPlayersExceededException(string message) : base(message)
    {
    }
}
