using System;
using TelephoneDirectory.Shared.Dtos;
using TelephoneDirectory.Shared.Messages;

namespace TelephoneDirectory.Shared.Interfaces
{
	public interface IRabbitMqService
	{
		Task SendMessage(CreateReportMessageCommand createReportMessageCommand);
	}
}

