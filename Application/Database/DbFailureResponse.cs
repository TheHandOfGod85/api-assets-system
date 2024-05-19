namespace Application;

public class DbFailureResponse : Exception
{
    public DbFailureResponse(string message) : base(message)
    {
    }
}


