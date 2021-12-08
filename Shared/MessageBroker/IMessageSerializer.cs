using System;

namespace MessageBroker
{
	public interface IMessageSerializer
	{
		string Serialize(Object source);

		T DeSerialize<T>(string source);
	}
}
