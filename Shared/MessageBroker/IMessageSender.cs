using System.Threading.Tasks;

namespace MessageBroker
{
	public interface IMessageSender
	{
		Task SendMessageAsync<T>(T message);
	}
}