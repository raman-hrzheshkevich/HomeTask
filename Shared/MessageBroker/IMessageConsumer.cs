using System.Threading.Tasks;

namespace MessageBroker
{
	public interface IMessageConsumer
    {
        Task RegisterMessageHandlers();
        Task CloseQueueAsync();
        ValueTask DisposeAsync();
    }
}
