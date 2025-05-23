using CampusLearn.DataModel.Models;
using CampusLearn.DataModel.Models.Configurations;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using Newtonsoft.Json;
using CampusLearn.DataModel.ViewModels;
using Microsoft.Extensions.Options;

namespace CampusLearn.Services.Domain.Utils;

public class SMTPManager
{
    #region -- protected properties --
    protected readonly ILogger<SMTPManager> logger;
    protected readonly SMTPSettings smtpSettings;
    #endregion

    public SMTPManager(ILogger<SMTPManager> logger, IOptions<SMTPSettings> smtpSettings)
    {
        this.logger = logger;
        this.smtpSettings = smtpSettings.Value;
    }




    public async Task<GenericAPIResponse<object>> SendEmailAsync(MessageViewModel msg)
    {
        GenericAPIResponse<object> response = new GenericAPIResponse<object>();
        try
        {
            if (string.IsNullOrEmpty(smtpSettings.Host))
            {
                response.Status = false;
                response.StatusCode = -1;
                response.StatusMessage = "Host is missing in the configuration.";
                return response;
            }

            if (string.IsNullOrEmpty(smtpSettings.Username))
            {
                response.Status = false;
                response.StatusCode = -1;
                response.StatusMessage = "SMPT account username is missing in the configuration.";
                return response;
            }

            if (string.IsNullOrEmpty(smtpSettings.Password))
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
                logger.LogError("Email address not correctly formulated", "EmailEngineService v2 SendEmailAsync", new Exception($"Invalid email address detected {msg.SenderEmailAddress} while attempting to send email with id {msg.ID.ToString()}."));
                return response;
            }


            if (!ValidationUtils.IsValidEmailAddress(msg.ReceiverEmailAddress))
            {
                response.Status = false;
                response.StatusCode = -1;
                response.StatusMessage = "Invalid recipient email address.";
                logger.LogError("Email address not correctly formulated", "EmailEngineService v2 SendEmailAsync", new Exception($"Invalid email address detected {msg.ReceiverEmailAddress} while attempting to send email with id {msg.ID.ToString()}."));
                return response;
            }

            SmtpClient client = new SmtpClient(smtpSettings.Host, smtpSettings.Port);
            //client.Credentials = CredentialCache.DefaultNetworkCredentials;
            client.Credentials = new System.Net.NetworkCredential(smtpSettings.Username, smtpSettings.Password);
            client.EnableSsl = true;

            MailAddress fromAddress = new MailAddress(smtpSettings.Username, $"{smtpSettings.DisplayName}", System.Text.Encoding.UTF8);
            MailMessage message = new MailMessage();
            message.From = fromAddress;
            message.Priority = MailPriority.High;
            message.To.Add(msg.ReceiverEmailAddress);
            message.CC.Add(msg.SenderEmailAddress);
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
