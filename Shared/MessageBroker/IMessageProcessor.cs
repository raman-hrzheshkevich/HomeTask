using System.Threading.Tasks;

namespace MessageBroker
{
	public interface IMessageProcessor<T> where T : class
	{
		public Task ProcessMessage(T message);
	}
}
