using System;

namespace Barista.Contracts.Commands.SpendingLimit
{
    public interface IUpdateSpendingLimit : ICommand
    {
        Guid Id { get; }
        Guid ParentUserAssignmentId { get; }
        TimeSpan Interval { get; }
        decimal Value { get; }
    }
}
