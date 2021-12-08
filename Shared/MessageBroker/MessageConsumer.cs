using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MessageBroker
{
	public class MessageConsumer<T> : IMessageConsumer where T : class
	{
		private const string QUEUE_NAME = "catalogqueue";

		private readonly IConfiguration configuration;
		private readonly ServiceBusClient client;
		private readonly ILogger<MessageConsumer<T>> logger;
		private readonly IMessageSerializer messageSerializer;
		private readonly IMessageProcessor<T> messageProcessor;
		private ServiceBusProcessor processor;

		public MessageConsumer(IConfiguration configuration, ILogger<MessageConsumer<T>> logger, IMessageSerializer messageSerializer, IMessageProcessor<T> messageProcessor)
		{
			this.configuration = configuration;
			this.logger = logger;
			this.messageSerializer = messageSerializer;
			this.messageProcessor = messageProcessor;
			var connectionString = this.configuration.GetConnectionString("ServiceBusConnectionString");
			client = new ServiceBusClient(connectionString);
		}

		public async Task RegisterMessageHandlers()
		{
			ServiceBusProcessorOptions serviceBusOptions = new ServiceBusProcessorOptions { AutoCompleteMessages = false };

			processor = client.CreateProcessor(QUEUE_NAME, serviceBusOptions);
			processor.ProcessMessageAsync += ProcessMessagesAsync;
			processor.ProcessErrorAsync += ProcessErrorAsync;
			await processor.StartProcessingAsync().ConfigureAwait(false);
		}

		private Task ProcessErrorAsync(ProcessErrorEventArgs arg)
		{
			logger.LogError(arg.Exception, "Message handler encountered an exception");
			logger.LogDebug($"- ErrorSource: {arg.ErrorSource}");
			logger.LogDebug($"- Entity Path: {arg.EntityPath}");
			logger.LogDebug($"- FullyQualifiedNamespace: {arg.FullyQualifiedNamespace}");

			return Task.CompletedTask;
		}

		private async Task ProcessMessagesAsync(ProcessMessageEventArgs args)
		{
			var message = messageSerializer.DeSerialize<T>(args.Message.Body.ToString());

			await messageProcessor.ProcessMessage((T)message);

			await args.CompleteMessageAsync(args.Message).ConfigureAwait(false);
		}

		public async ValueTask DisposeAsync()
		{
			if (processor != null)
			{
				await processor.DisposeAsync().ConfigureAwait(false);
			}

			if (client != null)
			{
				await client.DisposeAsync().ConfigureAwait(false);
			}
		}

		public async Task CloseQueueAsync()
		{
			await processor.CloseAsync().ConfigureAwait(false);
		}
	}
}
