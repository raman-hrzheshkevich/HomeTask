using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace MessageBroker
{
	public class MessageSender : IMessageSender
	{
		private const string QUEUE_NAME = "catalogqueue";

		private readonly ServiceBusClient client;
		private readonly ServiceBusSender clientSender;
		private readonly IMessageSerializer serializer;

		public MessageSender(IConfiguration configuration, IMessageSerializer serializer)
		{
			var connectionString = configuration.GetConnectionString("ServiceBusConnectionString");

			client = new ServiceBusClient(connectionString);
			clientSender = client.CreateSender(QUEUE_NAME);
			this.serializer = serializer;
		}

		public async Task SendMessageAsync<T>(T payload)
		{
			string messagePayload = serializer.Serialize(payload);
			ServiceBusMessage message = new ServiceBusMessage(messagePayload);
			await clientSender.SendMessageAsync(message).ConfigureAwait(false);
		}
	}
}
