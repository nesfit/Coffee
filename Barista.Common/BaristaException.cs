using System;
using System.Net;

namespace Barista.Common
{
    public class BaristaException : ApplicationException
    {
        public string Code { get; }

        public HttpStatusCode? RecommendedStatusCode
        {
            get
            {
                if (Code?.StartsWith("invalid_") == true)
                    return HttpStatusCode.BadRequest;
                else if (Code?.EndsWith("_not_found") == true)
                    return HttpStatusCode.NotFound;
                else if (Code?.Contains("authentication") == true && Code?.Contains("failed") == true)
                    return HttpStatusCode.Unauthorized;

                return null;
            }
        }

        public BaristaException(string code, string message) : base(message)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }

        public BaristaException(string code, string message, Exception innerException) : base(message, innerException)
        {
            Code = code ?? throw new ArgumentNullException(nameof(code));
        }
    }
}
