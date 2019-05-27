using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;

namespace Barista.Common
{
    public static class Extensions
    {
        /// (c) 2018 DevMentors, released under the MIT license at https://github.com/devmentors/DNC-DShop.Common/blob/780e35c4c3621aec937120fb8d1eee73ed048678/src/DShop.Common/Mvc/Extensions.cs

        public static T Bind<T>(this T model, Expression<Func<T, Guid>> expression, Guid value)
            => model.BindInternal<T, Guid>(expression, value);

        public static T Bind<T>(this T model, Expression<Func<T, Guid?>> expression, Guid? value)
            => model.BindInternal<T, Guid?>(expression, value);

        public static T Bind<T>(this T model, Expression<Func<T, int>> expression, int value)
            => model.BindInternal<T, int>(expression, value);

        internal static T BindInternal<T,V>(this T model, Expression<Func<T, V>> expression, V value)
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression == null)
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            var propertyName = memberExpression.Member.Name.ToLowerInvariant();
            var modelType = model.GetType();
            var field = modelType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                .SingleOrDefault(x => x.Name.ToLowerInvariant().StartsWith($"<{propertyName}>"));
            if (field == null)
            {
                return model;
            }

            field.SetValue(model, value);

            return model;
        }

        public static string UnderscorizePascalCamelCase(this Type type, string namespaceToOmit = null)
        {
            var typeNs = type.Namespace;
            var typeName = type.Name;

            if (namespaceToOmit != null)
                if (typeNs.StartsWith(namespaceToOmit))
                    typeNs = typeNs.Substring(namespaceToOmit.Length).Trim('.');
                else
                    throw new Exception();

            if (type.IsInterface && typeName.StartsWith('I'))
                typeName = typeName.Substring(1);

            string Underscorize(string s) => Regex.Replace(s, "(.)([A-Z])", "$1_$2");

            var nameSegment = Underscorize(typeName);
            var nsSegments = typeNs.Split(new[] {'.'}).Select(Underscorize);
            
            return string.Concat(string.Join('.', nsSegments), ".", typeName).ToLowerInvariant();
        }

        public static IQueryable<T> ApplySort<T>(this IQueryable<T> queryable, string[] sortByRequests, params string[] fallbackSortBy)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            if (sortByRequests == null) throw new ArgumentNullException(nameof(sortByRequests));

            if (typeof(IIdentifiable).IsAssignableFrom(typeof(T)) && fallbackSortBy.Length == 0)
                fallbackSortBy = fallbackSortBy.Concat(new[] {"Id ASC"}).ToArray();

            IOrderedQueryable<T> orderedQueryable = null;

            foreach (var sortByRequest in sortByRequests.Concat(fallbackSortBy))
            {
                // https://stackoverflow.com/a/21936366, dostupne pod licenci CC BY-SA 3.0

                var requestSegments = sortByRequest.Split(' ');
                if (requestSegments.Length != 2)
                    throw new BaristaException("invalid_sort_by_request", $"Could not parse sorting request '{sortByRequest}', column name and sorting order (ASC or DESC) separated by a space were expected.");

                var sortByColumn = requestSegments[0];
                var sortByOrder = requestSegments[1].ToLowerInvariant();
                var descendingSortRequested = false;

                switch (sortByOrder)
                {
                    case "asc":
                        break;

                    case "desc":
                        descendingSortRequested = true;
                        break;

                    default:
                        throw new BaristaException("invalid_sort_by_order", $"Could not parse sorting order '{sortByOrder}', 'ASC' or 'DESC' was expected.");
                }
                
                var param = Expression.Parameter(typeof(T));
                MemberExpression prop;

                try
                {
                    prop = Expression.Property(param, sortByColumn);
                }
                catch (ArgumentException e) when (e.ParamName == "propertyName")
                {
                    throw new BaristaException("invalid_sort_by_column", $"The column '{sortByColumn}' does not exist or does not support ordering.");
                }

                var propAsObj = Expression.Convert(prop, typeof(object));
                var lambda = Expression.Lambda<Func<T, object>>(propAsObj, param);

                if (orderedQueryable is null)
                    orderedQueryable = descendingSortRequested
                        ? queryable.OrderByDescending(lambda)
                        : queryable.OrderBy(lambda);
                else
                    orderedQueryable = descendingSortRequested
                        ? orderedQueryable.ThenByDescending(lambda)
                        : orderedQueryable.ThenBy(lambda);
            }

            return orderedQueryable;
        }

        public static TDto MapToWithNullPropagation<TDto>(this IMapper mapper, object src)
            where TDto : class
        {
            if (mapper == null) throw new ArgumentNullException(nameof(mapper));
            return src is null ? null : mapper.Map<TDto>(src);
        }
    }
}
