namespace Domain.Exceptions;

public class PlayerIsAlreadySignedUpForMatchException : BadRequestException
{
    public PlayerIsAlreadySignedUpForMatchException(string message) : base(message)
    {
    }
}
