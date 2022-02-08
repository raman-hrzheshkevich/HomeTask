using System.Threading.Tasks;

namespace MessageBroker
{
	//Test comment.
	public interface IMessageSender
	{
		Task SendMessageAsync<T>(T message);
	}
}
