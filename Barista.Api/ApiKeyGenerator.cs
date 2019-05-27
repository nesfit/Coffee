using System;
using System.Security.Cryptography;

namespace Barista.Api
{
    public class ApiKeyGenerator : IApiKeyGenerator
    {
        private readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        public const int KeySizeInBytes = 128;

        public string Generate()
        {
            var bytes = new byte[KeySizeInBytes];
            _rng.GetBytes(bytes, 0, KeySizeInBytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
