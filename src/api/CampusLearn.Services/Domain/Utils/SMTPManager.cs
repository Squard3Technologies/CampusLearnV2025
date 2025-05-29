namespace CampusLearn.Services.Domain.Utils;

public class SMTPManager
{
    private readonly ILogger<SMTPManager> _logger;
    private readonly SMTPSettings _smtpSettings;

    public SMTPManager(ILogger<SMTPManager> logger, IOptions<SMTPSettings> smtpSettings)
    {
        _logger = logger;
        _smtpSettings = smtpSettings.Value;
    }

    public async Task<GenericAPIResponse<object>> SendEmailAsync(NotificationViewModel msg)
    {
        GenericAPIResponse<object> response = new GenericAPIResponse<object>();
        try
        {
            if (string.IsNullOrEmpty(_smtpSettings.Host))
            {
                response.Status = false;
                response.StatusCode = -1;
                response.StatusMessage = "Host is missing in the configuration.";
                return response;
            }

            if (string.IsNullOrEmpty(_smtpSettings.Username))
            {
                response.Status = false;
                response.StatusCode = -1;
                response.StatusMessage = "SMPT account username is missing in the configuration.";
                return response;
            }

            if (string.IsNullOrEmpty(_smtpSettings.Password))
            {
                response.Status = false;
                response.StatusCode = -1;
                response.StatusMessage = "SMTP account password is missing in the configuration.";
                return response;
            }

            if (!ValidationUtils.IsValidEmailAddress(msg.SenderEmailAddress))
            {
                response.Status = false;
                response.StatusCode = -1;
                response.StatusMessage = "Invalid sender email address.";
                _logger.LogError("Email address not correctly formulated", "EmailEngineService v2 SendEmailAsync", new Exception($"Invalid email address detected {msg.SenderEmailAddress} while attempting to send email with id {msg.ID.ToString()}."));
                return response;
            }

            if (!ValidationUtils.IsValidEmailAddress(msg.ReceiverEmailAddress))
            {
                response.Status = false;
                response.StatusCode = -1;
                response.StatusMessage = "Invalid recipient email address.";
                _logger.LogError("Email address not correctly formulated", "EmailEngineService v2 SendEmailAsync", new Exception($"Invalid email address detected {msg.ReceiverEmailAddress} while attempting to send email with id {msg.ID.ToString()}."));
                return response;
            }

            SmtpClient client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port);
            //client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Credentials = new System.Net.NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
            client.EnableSsl = true;

            MailAddress fromAddress = new MailAddress(_smtpSettings.Username, $"{_smtpSettings.DisplayName}", System.Text.Encoding.UTF8);
            MailMessage message = new MailMessage();
            message.From = fromAddress;
            message.Priority = MailPriority.High;
            message.To.Add(new MailAddress(msg.ReceiverEmailAddress));
            message.CC.Add(new MailAddress(msg.SenderEmailAddress));
            message.IsBodyHtml = true;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = $"RE: MESSAGE FROM {msg.SenderName.ToUpper()} {msg.SenderSurname.ToUpper()}";
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Body = msg.MessageBody;

            string userState = msg.ID.ToString();
            await client.SendMailAsync(message);
            message.Dispose();
            client.Dispose();

            response.Status = true;
            response.StatusCode = 250;
            response.StatusMessage = "Email sent successfully";
        }
        catch (SmtpException e)
        {
            response.Status = false;
            response.StatusCode = (int)e.StatusCode;
            response.StatusMessage = e.Message;
            response.StatusDetailedMessage = JsonConvert.SerializeObject(e.Data);
        }
        catch (Exception ex)
        {
            response.Status = false;
            response.StatusCode = -1;
            response.StatusMessage = ex.Message;
            response.StatusDetailedMessage = JsonConvert.SerializeObject(ex.Data);
        }
        return response;
    }
}
