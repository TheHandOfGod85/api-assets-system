namespace SharedKernel;

public class FileUploadFailure : Exception
{
    public FileUploadFailure()
    {

    }
    public FileUploadFailure(string message) : base(message)
    {

    }

}
