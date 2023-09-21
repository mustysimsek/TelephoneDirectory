using System;
using MassTransit;
using TelephoneDirectory.Shared.Interfaces;
using TelephoneDirectory.Shared.Messages;

namespace TelephoneDirectory.Shared.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public RabbitMqService(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task SendMessage(CreateReportMessageCommand createReportMessageCommand)
        {
            var sendEndpoint = await _sendEndpointProvider.
                GetSendEndpoint(new Uri("queue:create-report-service"));
            await sendEndpoint.Send<CreateReportMessageCommand>(createReportMessageCommand);
        }
    }
}

