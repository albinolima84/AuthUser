using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Sky.Auth.Command.Responses
{
    public class Response<TValue>
    {
        public Response()
        {
            Messages = new Dictionary<string, string>();
        }

        public Response(TValue value) : this()
        {
            Value = value;
        }

        public TValue Value { get; }

        public IDictionary<string, string> Messages { get; }


        [JsonIgnore]
        public bool IsFailure => !IsSuccess;

        [JsonIgnore]
        public bool IsSuccess => Messages.Count == 0;

        public static Response<TValue> Ok(TValue value)
        {
            return new Response<TValue>(value);
        }

        public static Response<TValue> Fail(string property, string messsage)
        {
            var response = new Response<TValue>(default(TValue));
            response.Messages.Add(property, messsage);

            return response;
        }

        public static Response<TValue> Fail(IDictionary<string, string> messsages)
        {
            var response = new Response<TValue>(default(TValue));
            messsages.Keys.ToList().ForEach(k => response.Messages.Add(k, messsages[k]));

            return response;
        }
    }
}
