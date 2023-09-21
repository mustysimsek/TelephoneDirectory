using System;
using MassTransit;
using TelephoneDirectory.Shared.Interfaces;
using TelephoneDirectory.Shared.Messages;

namespace TelephoneDirectory.Shared.Services
{
    public class RabbitMqService : IRabbitMqService
    {
        //private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IBus _bus;

        public RabbitMqService(IBus bus)
        {
            _bus = bus;
        }
        public async Task SendMessage(CreateReportMessageCommand createReportMessageCommand)
        {
            var sendEndpoint = await _bus.GetSendEndpoint(new Uri("queue:create-report-service"));
            await sendEndpoint.Send(createReportMessageCommand);
        }
    }
}

