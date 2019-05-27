using System;
using Barista.Contracts;
using Barista.Users.Dto;

namespace Barista.Users.Queries
{
    public class GetUser : IQuery<UserDto>
    {
        public GetUser(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
