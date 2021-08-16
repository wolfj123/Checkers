using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Checkers_Server
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<TResult> Map<TSource, TResult>(this IEnumerable<TSource> self, Func<TSource, TResult> selector)
        {
            return self.Select(selector);
        }

        public static TSource Reduce<TSource>(this IEnumerable<TSource> self, Func<TSource, TSource, TSource> func)
        {
            return self.Aggregate(func);
        }

        public static TAccumulate Reduce<TSource, TAccumulate>(this IEnumerable<TSource> self, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            return self.Aggregate(seed, func);
        }

        public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> self, Func<TSource, bool> predicate)
        {
            return self.Where(predicate);
        }
    };
}
