namespace Domain;

public class DepartmentIsUniqueException : Exception
{
    public DepartmentIsUniqueException()
    {

    }

    public DepartmentIsUniqueException(string message) : base(message)
    {

    }

}
