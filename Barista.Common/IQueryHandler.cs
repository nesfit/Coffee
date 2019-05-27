using System.Threading.Tasks;
using Barista.Contracts;

namespace Barista.Common
{
    public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
