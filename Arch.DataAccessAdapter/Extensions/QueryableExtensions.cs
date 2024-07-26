using System.Linq.Expressions;

namespace Arch.DataAccessAdapter.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string columnName, bool isAscending = true)
        {
            if (string.IsNullOrEmpty(columnName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, string.Empty);
            MemberExpression property = Expression.Property(parameter, columnName);
            LambdaExpression keySelector = Expression.Lambda(property, parameter);
            string method = isAscending ? "OrderBy" : "OrderByDescending";

            Type[] types = new Type[] { source.ElementType, property.Type };

            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), method, types, source.Expression, Expression.Quote(keySelector));

            return source.Provider.CreateQuery<T>(resultExp);
        }
    }

}
