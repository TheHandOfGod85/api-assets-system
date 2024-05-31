using Application;
using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Model;


namespace Infrastructure;

public class EmailSender(IOptions<MailSettings> options) : IEmailSender
{
    private readonly MailSettings _mailSettings = options.Value;

    public void SendEmail(
        string email,
        string receiverName,
        string subject,
        string message,
        string? htmlContent)
    {
        var apiInstance = new TransactionalEmailsApi();
        SendSmtpEmailSender sender = new SendSmtpEmailSender(_mailSettings.SenderName, _mailSettings.SenderEmail);

        SendSmtpEmailTo receiver1 = new SendSmtpEmailTo(email, receiverName);
        List<SendSmtpEmailTo> To = [receiver1];
        string TextContent = message;

        try
        {
            var sendSmtpEmail = new SendSmtpEmail(sender, To, null, null, htmlContent, TextContent, subject);
            CreateSmtpEmail result = apiInstance.SendTransacEmail(sendSmtpEmail);
            Console.WriteLine("Brevo Response" + result.ToJson());
        }
        catch (Exception e)
        {
            Console.WriteLine("We have an exception" + e.Message);
        }
    }
}
