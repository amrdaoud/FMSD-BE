using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using System.Linq.Dynamic.Core;
using FMSD_BE.Dtos.ReportDtos.DistributionTransactionDtos;
using FMSD_BE.Models;

namespace FMSD_BE.Helper.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, GeneralFilterModel filterModel,List<string> searchFields)
        {
            if (!string.IsNullOrEmpty(filterModel.SearchQuery) && searchFields != null && searchFields.Any())
            {
                var parameter = Expression.Parameter(typeof(T), "x");
                var searchQuery = filterModel.SearchQuery.ToLower();
                Expression? searchExpression = null;

                foreach (var field in searchFields)
                {
                    // Get the nested property
                    var property = GetNestedProperty(typeof(T), field);
                    if (property == null) continue; // Skip if property path is invalid

                    // Create property access expressions for nested properties (e.g., x.User.Name)
                    Expression propertyAccess = parameter;
                    foreach (var part in field.Split('.'))
                    {
                        propertyAccess = Expression.Property(propertyAccess, part);
                    }

                    // Convert to string and apply ToLower
                    var toStringMethod = typeof(object).GetMethod("ToString")!;
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;
                    var toStringCall = Expression.Call(propertyAccess, toStringMethod);
                    var toLowerCall = Expression.Call(toStringCall, toLowerMethod);

                    // Build the Contains expression
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                    var searchTerm = Expression.Constant(searchQuery);
                    var containsExpression = Expression.Call(toLowerCall, containsMethod, searchTerm);

                    // Combine expressions with OR
                    searchExpression = searchExpression == null
                        ? containsExpression
                        : Expression.OrElse(searchExpression, containsExpression);
                }

                if (searchExpression != null)
                {
                    var lambda = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
                    query = query.Where(lambda);
                }
            }

            return query;
        }

        public static List<T> ApplySorting<T>(this List<T> list, GeneralFilterModel filterModel)
        {
            if (!string.IsNullOrEmpty(filterModel.SortActive))
            {
                // Determine the sorting direction
                string direction = filterModel.SortDirection?.ToLower() == "desc" ? "descending" : "ascending";

                // Use System.Linq.Dynamic.Core for dynamic sorting on the view model
                list = list.AsQueryable().OrderBy($"{filterModel.SortActive} {direction}").ToList();
            }
            return list;
        }

        public static  PagedResult<T> ToPagedResult<T>(this IQueryable<T> query, GeneralFilterModel filterModel)
        {
            var totalCount =  query.Count();
            var items =  query
                .Skip(filterModel.PageIndex * filterModel.PageSize)
                .Take(filterModel.PageSize)
                .ToList();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                PageIndex = filterModel.PageIndex,
                PageSize = filterModel.PageSize
            };
        }

        public static PropertyInfo? GetNestedProperty(Type type, string propertyPath)
        {
            PropertyInfo? property = null;
            foreach (var part in propertyPath.Split('.'))
            {
                property = type.GetProperty(part, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (property == null)
                    return null; // Property path is invalid
                type = property.PropertyType;
            }
            return property;
        }

        public static IQueryable<T> ApplySortingQuerable<T>(this IQueryable<T> list, GeneralFilterModel filterModel)
        {
            if (!string.IsNullOrEmpty(filterModel.SortActive))
            {
                // Determine the sorting direction
                string direction = filterModel.SortDirection?.ToLower() == "desc" ? "descending" : "ascending";

                if(direction == "ascending")
                {
                    list = list.OrderBy2(filterModel.SortActive);

                }
                else
                {
                    list = list.OrderByDescending2(filterModel.SortActive);

                }
            }
            return list;
        }

        public static IOrderedQueryable<T> OrderBy2<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderBy(toLambda<T>(propertyName));
        }

        public static IOrderedQueryable<T> OrderByDescending2<T>(this IQueryable<T> source, string propertyName)
        {
            return source.OrderByDescending(toLambda<T>(propertyName));
        }

        private static Expression<Func<T, object>> toLambda<T>(string propertyName)
        {
            var parameter = Expression.Parameter(typeof(T));
            var property = Expression.Property(parameter, propertyName);
            var propertyAsObject = Expression.Convert(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(propertyAsObject, parameter);
        }

       

    }
}
