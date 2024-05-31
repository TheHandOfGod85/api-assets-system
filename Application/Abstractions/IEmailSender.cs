namespace Application;

public interface IEmailSender
{
    void SendEmail(
            string email,
            string receiverName,
            string subject,
            string message,
            string? htmlContent);
}
