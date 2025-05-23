using CampusLearn.DataModel.Models.Messages;
using CampusLearn.DataModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.Services.Domain.Emailing;

public interface IMessagingServices
{
    Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, string messageType);
    Task StartEmailingServiceAsync();

    bool EMAILServiceBusy();

    bool SMSServiceBusy();
}
