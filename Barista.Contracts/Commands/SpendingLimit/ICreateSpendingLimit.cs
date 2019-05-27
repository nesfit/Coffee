using System;

namespace Barista.Contracts.Commands.SpendingLimit
{
    public interface ICreateSpendingLimit : ICommand
    {
        Guid Id { get; }
        Guid ParentUserAssignmentId { get; }
        TimeSpan Interval { get; }
        decimal Value { get; }
    }
}
