using System;
using Barista.Api.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Barista.Api.Authenticators.Impl
{
    public class MeansValueHasher : IMeansValueHasher
    {
        private readonly IPasswordHasher<AuthenticationMeansWithValue> _passwordHasher;

        public MeansValueHasher(IPasswordHasher<AuthenticationMeansWithValue> passwordHasher)
        {
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        public string Hash(string originalValue) =>
            _passwordHasher.HashPassword(new AuthenticationMeansWithValue(), originalValue);
    }
}
