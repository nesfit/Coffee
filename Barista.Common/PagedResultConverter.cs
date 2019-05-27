using System.Collections.Generic;
using AutoMapper;
using Barista.Contracts;

namespace Barista.Common
{
    public class PagedResultConverter<TSource,TDestination> : ITypeConverter<IPagedResult<TSource>, IPagedResult<TDestination>>
    {
        public IPagedResult<TDestination> Convert(IPagedResult<TSource> source, IPagedResult<TDestination> destination, ResolutionContext context)
        {
            return new PagedResult<TDestination>(source, context.Mapper.Map<IEnumerable<TDestination>>(source.Items));
        }
    }
}
