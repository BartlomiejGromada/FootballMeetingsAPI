namespace Domain.Exceptions;

public class FootballPitchNameIsAlreadyTakenException : Exception
{
    public FootballPitchNameIsAlreadyTakenException(string message) : base(message)
    {

    }
}
