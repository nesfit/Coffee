using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Barista.Api.Authenticators;
using Barista.Api.Authorization;
using Barista.Api.Dto;
using Barista.Api.Models;
using Barista.Api.Services;
using Barista.Common;
using Barista.Common.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Barista.Api.Controllers
{
    [Route("api/token")]
    [ApiController]
    public class TokensController : BaseController
    {
        private readonly IConfiguration _configuration;

        public TokensController(IBusPublisher busPublisher, IConfiguration configuration) : base(busPublisher)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        private string GenerateToken(DateTimeOffset validUntil, params Claim[] claims)
        {
            var jwtSigningKeyBytes = Convert.FromBase64String(_configuration["Jwt:SigningKey"]);
            var jwtSigningKey = new SymmetricSecurityKey(jwtSigningKeyBytes);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                notBefore: DateTime.UtcNow,
                expires: validUntil.UtcDateTime,
                claims: claims,
                signingCredentials: new SigningCredentials(jwtSigningKey, SecurityAlgorithms.HmacSha256Signature)
            );

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            return jwtTokenHandler.WriteToken(tokenDescriptor);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthToken>> UserLogIn(UserLoginDto data, [FromServices] IUsersService usersService, [FromServices] IUserPasswordAuthenticator authenticator, [FromQuery]bool saveAsCookie = false)
        {
            var authResult = await authenticator.AuthenticateAsync(data.EmailAddress, data.Password);
            if (authResult is null)
                return Unauthorized(new ErrorDto("authentication_failed", "Invalid login credentials."));

            var user = await usersService.GetUser(authResult.SubjectId) ?? throw new BaristaException("authentication_failed", "Could not retrieve authenticated user, please try again.");
            var validUntil = DateTimeOffset.UtcNow.AddHours(6);

            var token = GenerateToken(validUntil, new[]
            {
                new Claim(Claims.UserId, authResult.SubjectId.ToString("D")),
                new Claim(Claims.MeansId, authResult.MeansId.ToString("D")),
                new Claim(Claims.IsAdministrator, user.IsAdministrator.ToString())
            });

            if (saveAsCookie)
                Response.Cookies.Append(JwtFromCookieLoadingMiddleware.JwtCookieName, token, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = validUntil,
                    SameSite = SameSiteMode.Lax,
                    Domain = _configuration["JwtCookieDomain"],
                    Secure = bool.Parse(_configuration["JwtCookieSecure"])
                });

            return new AuthToken(
                GenerateToken(validUntil, new[]
                {
                    new Claim(Claims.UserId, authResult.SubjectId.ToString("D")),
                    new Claim(Claims.MeansId, authResult.MeansId.ToString("D")),
                    new Claim(Claims.IsAdministrator, user.IsAdministrator.ToString())
                }),

                validUntil
            );
        }

        [Authorize]
        [HttpPost("logoutCookie")]
        public IActionResult LogoutCookie()
        {
            Response.Cookies.Delete(JwtFromCookieLoadingMiddleware.JwtCookieName, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Domain = _configuration["JwtCookieDomain"],
                Secure = bool.Parse(_configuration["JwtCookieSecure"])
            });

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("userApiKey")]
        public async Task<ActionResult<AuthToken>> UserApiKeyLogIn(UserApiKeyLoginDto data, [FromServices] IUserApiKeyAuthenticator authenticator, [FromServices] IUsersService usersService)
        {
            var authResult = await authenticator.AuthenticateAsync(data.UserId, data.Key);
            if (authResult is null)
                return Unauthorized(new ErrorDto("authentication_failed", "Invalid login credentials."));

            var user = await usersService.GetUser(authResult.SubjectId) ?? throw new BaristaException("authentication_failed", "Could not retrieve authenticated user, please try again.");
            var validUntil = DateTimeOffset.UtcNow.AddHours(24);

            return new AuthToken(
                GenerateToken(validUntil, new[]
                {
                    new Claim(Claims.UserId, authResult.SubjectId.ToString("D")),
                    new Claim(Claims.MeansId, authResult.MeansId.ToString("D")),
                    new Claim(Claims.IsAdministrator, user.IsAdministrator.ToString())
                }),

                validUntil
            );
        }

        [AllowAnonymous]
        [HttpPost("pointOfSaleApiKey")]
        public async Task<ActionResult<AuthToken>> PointOfSaleApiKeyLogIn(PosApiKeyLoginDto data, [FromServices] IPointOfSaleApiKeyAuthenticator authenticator)
        {
            var authResult = await authenticator.AuthenticateAsync(data.PointOfSaleId, data.Key);
            if (authResult is null)
                return Unauthorized(new ErrorDto("authentication_failed", "Invalid login credentials."));

            var validUntil = DateTimeOffset.UtcNow.AddHours(24);

            return new AuthToken(
                GenerateToken(validUntil, new[]
                {
                    new Claim(Claims.PointOfSaleId, authResult.SubjectId.ToString("D")),
                    new Claim(Claims.MeansId, authResult.MeansId.ToString("D")),
                }),

                validUntil
            );
        }
    }
}