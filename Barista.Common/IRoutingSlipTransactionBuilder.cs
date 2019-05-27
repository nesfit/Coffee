using System;
using System.Threading.Tasks;

namespace Barista.Common
{
    public interface IRoutingSlipTransactionBuilder
    {
        IRoutingSlipTransactionBuilder Add<TActivity>();
        IRoutingSlipTransactionBuilder Add<TActivity, TActivityParams>(TActivityParams @params);
        IRoutingSlipTransactionBuilder SetVariables(object variables);
        Task<Guid> StartAsync();
    }
}
