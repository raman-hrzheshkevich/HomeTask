using Newtonsoft.Json;
using System;

namespace MessageBroker
{
	public class JsonMessageSerializer : IMessageSerializer
	{
		public T DeSerialize<T>(string source)
		{
			return JsonConvert.DeserializeObject<T>(source);
		}

		public string Serialize(Object source)
		{
			return JsonConvert.SerializeObject(source);
		}
	}
}
