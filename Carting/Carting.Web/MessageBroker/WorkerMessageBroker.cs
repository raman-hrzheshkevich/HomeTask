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
    /// <summary>
    /// WorkerMessageBroker class.
    /// </summary>
    public class WorkerMessageBroker : IHostedService, IDisposable
    {
        private readonly ILogger<WorkerMessageBroker> logger;
        private readonly IMessageConsumer messageConsumer;

		/// <summary>Initializes a new instance of the <see cref="WorkerMessageBroker" /> class.</summary>
		/// <param name="factory">The factory.</param>
		/// <param name="logger">The logger.</param>
		public WorkerMessageBroker(
            IServiceScopeFactory factory,
            ILogger<WorkerMessageBroker> logger)
        {
            messageConsumer = factory.CreateScope().ServiceProvider.GetRequiredService<IMessageConsumer>();
            this.logger = logger;
        }

		/// <summary>Starts the asynchronous.</summary>
		/// <param name="stoppingToken">The stopping token.</param>
		public async Task StartAsync(CancellationToken stoppingToken)
        {
            logger.LogDebug("Starting the service bus queue consumer and the subscription");
            await messageConsumer.RegisterMessageHandlers().ConfigureAwait(false);
        }

		/// <summary>Stops the asynchronous.</summary>
		/// <param name="stoppingToken">The stopping token.</param>
		public async Task StopAsync(CancellationToken stoppingToken)
        {
            logger.LogDebug("Stopping the service bus queue consumer and the subscription");
            await messageConsumer.CloseQueueAsync().ConfigureAwait(false);
        }

		/// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
		public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

		/// <summary>Releases unmanaged and - optionally - managed resources.</summary>
		/// <param name="disposing">
		///   <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual async void Dispose(bool disposing)
        {
            if (disposing)
            {
                await messageConsumer.DisposeAsync().ConfigureAwait(false);
            }
        }
	}
}
