using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Common.RestEase
{
    public class ResponseDeserializer : global::RestEase.ResponseDeserializer
    {
        private readonly global::RestEase.ResponseDeserializer _impl = new JsonResponseDeserializer();

        public override T Deserialize<T>(string content, HttpResponseMessage response, ResponseDeserializerInfo info)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                // Return an empty page.
                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(ResultPage<>))
                    return (T) Activator.CreateInstance(typeof(ResultPage<>).MakeGenericType(typeof(T).GetGenericArguments().Single()));
                else
                    return default(T);
            }
            else if (!response.IsSuccessStatusCode)
            {
                throw new ApiException(response.RequestMessage.Method, response.RequestMessage.RequestUri,
                    response.StatusCode, response.ReasonPhrase, response.Headers, response.Content.Headers,
                    response.Content.ReadAsStringAsync().Result);
            }

            return _impl.Deserialize<T>(content, response, info);
        }
    }
}
