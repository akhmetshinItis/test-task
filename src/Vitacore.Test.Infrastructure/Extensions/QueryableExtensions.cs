using System.Globalization;
using System.Linq.Expressions;
using Vitacore.Test.Contracts.Pagination;

namespace Vitacore.Test.Infrastructure.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, IPaginationQuery paginationQuery)
        {
            var page = paginationQuery.Page <= 0
                ? PaginationDefaults.Page
                : paginationQuery.Page;

            var pageSize = paginationQuery.PageSize <= 0
                ? PaginationDefaults.PageSize
                : paginationQuery.PageSize;

            if (pageSize > PaginationDefaults.MaxPageSize)
            {
                pageSize = PaginationDefaults.MaxPageSize;
            }

            return query
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public static IQueryable<T> ApplyOrdering<T>(this IQueryable<T> query, IOrderByQuery orderByQuery)
        {
            if (string.IsNullOrWhiteSpace(orderByQuery.OrderBy))
            {
                return query;
            }

            var property = typeof(T).GetProperties()
                .FirstOrDefault(x => string.Equals(x.Name, orderByQuery.OrderBy, StringComparison.OrdinalIgnoreCase));

            if (property is null)
            {
                return query;
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var keySelector = Expression.Lambda(propertyAccess, parameter);
            var methodName = orderByQuery.IsAsc ? nameof(Queryable.OrderBy) : nameof(Queryable.OrderByDescending);

            var orderedExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                [typeof(T), property.PropertyType],
                query.Expression,
                Expression.Quote(keySelector));

            return query.Provider.CreateQuery<T>(orderedExpression);
        }

        public static IQueryable<T> Filter<T>(
            this IQueryable<T> source,
            string? filterValue,
            Expression<Func<T, string?>> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            if (string.IsNullOrWhiteSpace(filterValue))
            {
                return source;
            }

            var normalizedFilter = filterValue.Trim().ToLower(CultureInfo.InvariantCulture);
            var stringValueExpression = selector.Body;
            var parameter = selector.Parameters[0];

            var notNullExpression = Expression.NotEqual(
                stringValueExpression,
                Expression.Constant(null, typeof(string)));

            var normalizedFieldExpression = Expression.Call(
                stringValueExpression,
                typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!);

            var containsExpression = Expression.Call(
                normalizedFieldExpression,
                typeof(string).GetMethod(nameof(string.Contains), [typeof(string)])!,
                Expression.Constant(normalizedFilter));

            var predicateBody = Expression.AndAlso(notNullExpression, containsExpression);
            var predicate = Expression.Lambda<Func<T, bool>>(predicateBody, parameter);

            return source.Where(predicate);
        }
    }
}
