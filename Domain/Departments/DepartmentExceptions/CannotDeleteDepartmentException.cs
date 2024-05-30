namespace Domain;

public class CannotDeleteDepartmentException : Exception
{
    public CannotDeleteDepartmentException()
    {

    }
    public CannotDeleteDepartmentException(string message) : base(message)
    {

    }

}
