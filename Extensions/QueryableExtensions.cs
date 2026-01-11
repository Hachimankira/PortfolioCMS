

namespace PortfolioCMS.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? sortColumn, string? sortDirection)
        {
            if (string.IsNullOrWhiteSpace(sortColumn))
            {
                return query;
            }

            sortDirection = string.IsNullOrWhiteSpace(sortDirection) ? "asc" : sortDirection;
            
            // Requires System.Linq.Dynamic.Core for dynamic string-based sorting
            // If strictly creating manually, we'd need expressions. 
            // For now, assuming we might need to be simpler or add the package.
            // Since I cannot add packages, I will implement a simpler property lookup if Dynamic.Core isn't available,
            // but usually in these envs we might be limited.
            // Let's rely on standard reflection or check if we can use a simpler approach.
            
            // Ideally we use: query.OrderBy($"{sortColumn} {sortDirection}");
            // But we need to verify if System.Linq.Dynamic.Core is available.
            
            // Fallback to simple PropertyInfo check
            var propertyInfo = typeof(T).GetProperty(sortColumn, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            
            if (propertyInfo == null)
            {
                return query;
            }

            bool isAscending = sortDirection.ToLower() == "asc";
            
            // Limitation: This only works for direct properties, not nested.
            // Also simplistic implementation. Use expression trees for better performance/correctness.
            
            var param = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
            var prop = System.Linq.Expressions.Expression.Property(param, propertyInfo);
            var lambda = System.Linq.Expressions.Expression.Lambda(prop, param);
            
            string methodName = isAscending ? "OrderBy" : "OrderByDescending";
            var method = typeof(Queryable).GetMethods()
                .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
                .Single()
                .MakeGenericMethod(typeof(T), propertyInfo.PropertyType);
                
            return (IQueryable<T>)method.Invoke(null, new object[] { query, lambda });
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
    }
}
