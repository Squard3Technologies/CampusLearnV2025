using CampusLearn.DataLayer.IRepositoryService;
using CampusLearn.DataModel.Models.Messages;
using CampusLearn.DataModel.ViewModels;
using CampusLearn.Services.Domain.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Emailing;

public class MessagingServices : IMessagingServices
{
    protected readonly IMessagingRepository messagingRepository;
    protected readonly SMTPManager smtpManager;
    private bool EmailerBusy = false;
    private bool SMSBusy = false;

    public MessagingServices(IMessagingRepository messagingRepository, SMTPManager smtpManager)
    {
        this.messagingRepository = messagingRepository;
        this.smtpManager = smtpManager;
    }

    public bool EMAILServiceBusy() => EmailerBusy;

    public bool SMSServiceBusy() => SMSBusy;


    public async Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, string messageType)
    {
        return await messagingRepository.SendMessageAsync(model, messageType);
    }

    public async Task StartEmailingServiceAsync()
    {
        EmailerBusy = true;
        try
        {
            var getMessageResponse = await messagingRepository.GetAllPendingMessagesAsync("EMAIL", 200);
            if(getMessageResponse.Status)
            {
                var messages = (List<MessageViewModel>)getMessageResponse.Body;
                foreach(var message in messages)
                {
                    var sendMessageResponse = await smtpManager.SendEmailAsync(message);
                    await messagingRepository.UpdateMessageStatusAsync(message, sendMessageResponse.StatusCode, sendMessageResponse.StatusMessage);
                }
            }
        }
        catch (Exception ex)
        {

        }
        EmailerBusy = false;
    }

}
