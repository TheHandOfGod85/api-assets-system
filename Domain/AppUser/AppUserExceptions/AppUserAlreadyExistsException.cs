namespace Domain;

public class AppUserAlreadyExistsException : Exception
{
    public AppUserAlreadyExistsException()
    {

    }

    public AppUserAlreadyExistsException(string message) : base(message)
    {

    }

}
