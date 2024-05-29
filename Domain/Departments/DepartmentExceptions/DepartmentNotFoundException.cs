namespace Domain;

public class DepartmentNotFoundException : Exception
{
    public DepartmentNotFoundException()
    {

    }

    public DepartmentNotFoundException(string message) : base(message)
    {

    }

}
