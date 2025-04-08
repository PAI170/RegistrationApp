using System.Linq.Expressions;

namespace RegistrationAPI.ExtensionMethods
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Search<T>(
            this IQueryable<T> query,
            string[] values,
            Func<string, Expression<Func<T, bool>>> predicateBuilder,
            List<Expression<Func<T, object>>>? orderBys = null)
        {
            if (values == null || values.Length == 0)
                return query;

            // Apply OR conditions for each search value
            var predicate = PredicateBuilder.False<T>();
            foreach (var value in values.Where(v => !string.IsNullOrWhiteSpace(v)))
            {
                predicate = predicate.Or(predicateBuilder(value));
            }

            query = query.Where(predicate);

            // Apply ordering if specified
            if (orderBys != null && orderBys.Count > 0)
            {
                IOrderedQueryable<T>? orderedQuery = null;
                for (int i = 0; i < orderBys.Count; i++)
                {
                    if (i == 0)
                        orderedQuery = query.OrderBy(orderBys[i]);
                    else
                        orderedQuery = orderedQuery.ThenBy(orderBys[i]);
                }
                return orderedQuery ?? query;
            }

            return query;
        }
    }

    // PredicateBuilder for dynamic OR/AND conditions
    public static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        public static Expression<Func<T, bool>> False<T>() { return f => false; }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1,
                                                        Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                  (Expression.OrElse(expr1.Body, invokedExpr), expr1.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1,
                                                         Expression<Func<T, bool>> expr2)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>
                  (Expression.AndAlso(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }
}
