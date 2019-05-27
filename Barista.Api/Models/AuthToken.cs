using System;

namespace Barista.Api.Models
{
    public class AuthToken
    {
        public AuthToken(string token, DateTimeOffset expires)
        {
            Token = token;
            Expires = expires;
        }

        public string Token { get; }
        public DateTimeOffset Expires { get; }
    }
}
