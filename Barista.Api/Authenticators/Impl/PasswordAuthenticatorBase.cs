using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Barista.Api.Authenticators.Impl
{
    public abstract class PasswordAuthenticatorBase<T> where T : class
    {
        private readonly IPasswordHasher<T> _passwordHasher;

        protected PasswordAuthenticatorBase(IPasswordHasher<T> passwordHasher)
        {
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        protected abstract Task UpdateHashedPasswordAsync(T passwordContainer, string newHashedApiKey);

        protected async Task<bool> Verify(T passwordContainer, string containedPassword, string providedPassword)
        {
            var verificationResult =
                _passwordHasher.VerifyHashedPassword(passwordContainer, containedPassword, providedPassword);

            switch (verificationResult)
            {
                case PasswordVerificationResult.Failed:
                    return false;

                case PasswordVerificationResult.Success:
                    return true;

                case PasswordVerificationResult.SuccessRehashNeeded:
                    await UpdatePassword(passwordContainer, providedPassword);
                    return true;

                default:
                    throw new NotSupportedException($"Unsupported PasswordVerificationResult: {verificationResult}");
            }
        }

        protected async Task UpdatePassword(T passwordContainer, string newPassword)
            => await UpdateHashedPasswordAsync(passwordContainer, _passwordHasher.HashPassword(passwordContainer, newPassword));
    }
}
