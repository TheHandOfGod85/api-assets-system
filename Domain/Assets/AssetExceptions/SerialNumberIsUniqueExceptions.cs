namespace Domain;

public class SerialNumberIsUniqueExceptions : Exception
{
    public SerialNumberIsUniqueExceptions()
    {

    }

    public SerialNumberIsUniqueExceptions(string message) : base(message)
    { }

}
