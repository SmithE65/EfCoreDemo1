using System;
using System.Collections.Generic;
using System.Linq;

namespace EfCoreDemo1.Models.Extensions
{
    public static class LinqExtensions
    {
        public static IQueryable<TOut> LeftJoin<TLeft, TRight, TOut, TKey>(
            this IQueryable<TLeft> lefts, IQueryable<TRight> inner, 
            Func<TLeft, TKey> outerKeySelector, 
            Func<TRight, TKey> innerKeySelector, 
            Func<(TLeft Outer, IEnumerable<TRight> Inner), TRight, TOut> resultSelector)
            => lefts.GroupJoin(
                    inner: inner,
                    outerKeySelector: outerKeySelector,
                    innerKeySelector: innerKeySelector,
                    resultSelector: (o, i) => ( Outer: o, Inner: i ))
                .SelectMany(x => x.Inner.DefaultIfEmpty(), resultSelector)
                .AsQueryable();
    }
}
