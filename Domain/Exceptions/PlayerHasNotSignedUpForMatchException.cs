namespace Domain.Exceptions;

public class PlayerHasNotSignedUpForMatchException : BadRequestException
{
    public PlayerHasNotSignedUpForMatchException(string message) : base(message)
    {
    }
}
