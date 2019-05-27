using System;
using System.Threading.Tasks;
using Barista.Api.Models.Users;
using Barista.Api.Queries;
using Barista.Common.Dto;
using RestEase;

namespace Barista.Api.Services
{
    [SerializationMethods(Query = QuerySerializationMethod.Serialized)]
    public interface IUsersService
    {
        [AllowAnyStatusCode]
        [Get("api/users")]
        Task<ResultPage<FullUser>> BrowseUsers([Query] BrowseUsers query);

        [AllowAnyStatusCode]
        [Get("api/users")]
        Task<ResultPage<FullUser>> BrowseDetailedUsers([Query] BrowseDetailedUsers query);

        [AllowAnyStatusCode]
        [Get("api/users/{id}")]
        Task<FullUser> GetUser([Path] Guid id);
    }
}
