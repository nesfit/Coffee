using System;
using System.ComponentModel.DataAnnotations;
using Barista.Contracts.Commands.AuthenticationMeans;

namespace Barista.Api.Commands.AuthenticationMeans
{
    public class UpdateAuthenticationMeansValue : IUpdateAuthenticationMeansValue
    {
        public UpdateAuthenticationMeansValue(Guid id, string value)
        {
            Id = id;
            Value = value;
        }

        public Guid Id { get; }
        public string Value { get; }
    }
}
