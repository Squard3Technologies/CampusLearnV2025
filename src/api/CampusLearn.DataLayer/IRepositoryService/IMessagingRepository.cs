using CampusLearn.DataModel.Models.Messages;
using CampusLearn.DataModel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CampusLearn.DataLayer.IRepositoryService;

public interface IMessagingRepository
{
    Task<GenericDbResponseViewModel> SendMessageAsync(SendMessageRequest model, string messageType);

    Task<GenericDbResponseViewModel> GetAllPendingMessagesAsync(string messageType, int batchSize = 100);

    Task UpdateMessageStatusAsync(MessageViewModel model, int statusCode, string statusMessage);

}
