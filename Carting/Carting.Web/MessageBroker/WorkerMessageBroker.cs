using MessageBroker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Carting.Web.MessageBroker
{
	public class WorkerMessageBroker : IHostedService, IDisposable
    {
        private readonly ILogger<WorkerMessageBroker> logger;
        private readonly IMessageConsumer messageConsumer;

        public WorkerMessageBroker(
            IServiceScopeFactory factory,
            ILogger<WorkerMessageBroker> logger)
        {
            messageConsumer = factory.CreateScope().ServiceProvider.GetRequiredService<IMessageConsumer>();
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogDebug("Starting the service bus queue consumer and the subscription");
            await messageConsumer.RegisterMessageHandlers().ConfigureAwait(false);
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogDebug("Stopping the service bus queue consumer and the subscription");
            await messageConsumer.CloseQueueAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (disposing)
            {
                await messageConsumer.DisposeAsync().ConfigureAwait(false);
            }
        }
	}
}
