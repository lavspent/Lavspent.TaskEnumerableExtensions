/*
    Copyright(c) 2017 Petter Labråten/LAVSPENT.NO. All rights reserved.

    The MIT License(MIT)

    Permission is hereby granted, free of charge, to any person obtaining a
    copy of this software and associated documentation files (the "Software"),
    to deal in the Software without restriction, including without limitation
    the rights to use, copy, modify, merge, publish, distribute, sublicense,
    and/or sell copies of the Software, and to permit persons to whom the
    Software is furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
    FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
    DEALINGS IN THE SOFTWARE.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;


namespace Lavspent.TaskEnumerableExtensions
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class Extensions
    {

        private static ConfiguredTaskAwaitable<IEnumerable<TSource>> Cfg<TSource>(Task<IEnumerable<TSource>> source)
        {
            return source.ConfigureAwait(false);
        }

        #region Where

        public static async Task<IEnumerable<TSource>> Where<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).Where(predicate);
        }

        public static async Task<IEnumerable<TSource>> Where<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, int, bool> predicate)
        {
            return (await Cfg(source)).Where(predicate);
        }

        #endregion

        #region Select/SelectMany

        public static async Task<IEnumerable<TResult>> Select<TSource, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, TResult> selector)
        {
            return (await Cfg(source)).Select(selector);
        }

        public static async Task<IEnumerable<TResult>> Select<TSource, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, int, TResult> selector)
        {
            return (await Cfg(source)).Select(selector);
        }

        public static async Task<IEnumerable<TResult>> SelectMany<TSource, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return (await Cfg(source)).SelectMany(selector);
        }

        public static async Task<IEnumerable<TResult>> SelectMany<TSource, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            return (await Cfg(source)).SelectMany(selector);
        }

        public static async Task<IEnumerable<TResult>> SelectMany<TSource, TCollection, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return (await Cfg(source)).SelectMany(collectionSelector, resultSelector);
        }

        public static async Task<IEnumerable<TResult>> SelectMany<TSource, TCollection, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return (await Cfg(source)).SelectMany(collectionSelector, resultSelector);
        }

        #endregion

        #region Take/TakeWhile

        public static async Task<IEnumerable<TSource>> Take<TSource>(this Task<IEnumerable<TSource>> source, int count)
        {
            return (await Cfg(source)).Take(count);
        }

        public static async Task<IEnumerable<TSource>> TakeWhile<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).TakeWhile(predicate);
        }
        public static async Task<IEnumerable<TSource>> TakeWhile<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, int, bool> predicate)
        {
            return (await Cfg(source)).TakeWhile(predicate);
        }

        #endregion

        #region Skip/SkipWhile

        public static async Task<IEnumerable<TSource>> Skip<TSource>(this Task<IEnumerable<TSource>> source, int count)
        {
            return (await Cfg(source)).Skip(count);
        }

        public static async Task<IEnumerable<TSource>> SkipWhile<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).SkipWhile(predicate);
        }

        public static async Task<IEnumerable<TSource>> SkipWhile<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, int, bool> predicate)
        {
            return (await Cfg(source)).SkipWhile(predicate);
        }

        #endregion

        #region Join

        public static async Task<IEnumerable<TResult>> Join<TOuter, TInner, TKey, TResult>(this Task<IEnumerable<TOuter>> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            return (await Cfg(outer)).Join(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static async Task<IEnumerable<TResult>> Join<TOuter, TInner, TKey, TResult>(this Task<IEnumerable<TOuter>> outer, Task<IEnumerable<TInner>> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            await Task.WhenAll(outer, inner).ConfigureAwait(false);
            return outer.Result.Join(inner.Result, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static async Task<IEnumerable<TResult>> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, Task<IEnumerable<TInner>> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector)
        {
            return outer.Join(await Cfg(inner), outerKeySelector, innerKeySelector, resultSelector);
        }

        public static async Task<IEnumerable<TResult>> Join<TOuter, TInner, TKey, TResult>(this Task<IEnumerable<TOuter>> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return (await Cfg(outer)).Join(inner, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static async Task<IEnumerable<TResult>> Join<TOuter, TInner, TKey, TResult>(this Task<IEnumerable<TOuter>> outer, Task<IEnumerable<TInner>> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            await Task.WhenAll(outer, inner).ConfigureAwait(false);
            return outer.Result.Join(inner.Result, outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        public static async Task<IEnumerable<TResult>> Join<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, Task<IEnumerable<TInner>> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, TInner, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        {
            return outer.Join(await Cfg(inner), outerKeySelector, innerKeySelector, resultSelector, comparer);
        }

        #endregion

        #region GroupJoin

        public static async Task<IEnumerable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this Task<IEnumerable<TOuter>> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
        {
            return (await Cfg(outer)).GroupJoin(inner, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static async Task<IEnumerable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this Task<IEnumerable<TOuter>> outer, Task<IEnumerable<TInner>> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
        {
            await Task.WhenAll(outer, inner).ConfigureAwait(false);
            return outer.Result.GroupJoin(inner.Result, outerKeySelector, innerKeySelector, resultSelector);
        }

        public static async Task<IEnumerable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, Task<IEnumerable<TInner>> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector)
        {
            return outer.GroupJoin(await Cfg(inner), outerKeySelector, innerKeySelector, resultSelector);
        }

        #endregion


        //public static async Task<IEnumerable<TResult>> GroupJoin<TOuter, TInner, TKey, TResult>(this IEnumerable<TOuter> outer, IEnumerable<TInner> inner, Func<TOuter, TKey> outerKeySelector, Func<TInner, TKey> innerKeySelector, Func<TOuter, IEnumerable<TInner>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        //{
        //}

        //public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
        //{
        //}

        //public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        //{
        //}

        //public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
        //{
        //}

        //public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        //{
        //}

        //public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        //{
        //}

        //public static IOrderedEnumerable<TSource> ThenBy<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        //{
        //}

        //public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        //{
        //}

        //public static IOrderedEnumerable<TSource> ThenByDescending<TSource, TKey>(this IOrderedEnumerable<TSource> source, Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        //{
        //}

        //public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
        //{
        //}

        //public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        //{
        //}

        //public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        //{
        //}

        //public static IEnumerable<IGrouping<TKey, TElement>> GroupBy<TSource, TKey, TElement>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        //{
        //}

        //public static async Task<IEnumerable<TResult>> GroupBy<TSource, TKey, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector)
        //{
        //}

        //public static async Task<IEnumerable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector)
        //{
        //}

        //public static async Task<IEnumerable<TResult>> GroupBy<TSource, TKey, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, Func<TKey, IEnumerable<TSource>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        //{
        //}

        //public static async Task<IEnumerable<TResult>> GroupBy<TSource, TKey, TElement, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TKey, IEnumerable<TElement>, TResult> resultSelector, IEqualityComparer<TKey> comparer)
        //{
        //}


        #region Concat

        public static async Task<IEnumerable<TSource>> Concat<TSource>(this Task<IEnumerable<TSource>> first, IEnumerable<TSource> second)
        {
            return (await Cfg(first)).Concat(second);
        }

        public static async Task<IEnumerable<TSource>> Concat<TSource>(this Task<IEnumerable<TSource>> first, Task<IEnumerable<TSource>> second)
        {
            await Task.WhenAll(first, second).ConfigureAwait(false);
            return first.Result.Concat(second.Result);
        }

        public static async Task<IEnumerable<TSource>> Concat<TSource>(this IEnumerable<TSource> first, Task<IEnumerable<TSource>> second)
        {
            return first.Concat(await Cfg(second));
        }

        #endregion


        //public static async Task<IEnumerable<TResult>> Zip<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst, TSecond, TResult> resultSelector)
        //{
        //}

        //public static async Task<IEnumerable<TSource>> Distinct<TSource>(this Task<IEnumerable<TSource>> source)
        //{
        //}

        //public static async Task<IEnumerable<TSource>> Distinct<TSource>(this Task<IEnumerable<TSource>> source, IEqualityComparer<TSource> comparer)
        //{
        //}

        //static IEnumerable<TSource> DistinctIterator<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        //{
        //}

        //public static async Task<IEnumerable<TSource>> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        //{
        //}

        //public static async Task<IEnumerable<TSource>> Union<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        //{
        //}

        //public static async Task<IEnumerable<TSource>> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        //{
        //}

        //public static async Task<IEnumerable<TSource>> Intersect<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        //{
        //}

        //public static async Task<IEnumerable<TSource>> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        //{
        //}

        //public static async Task<IEnumerable<TSource>> Except<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        //{
        //}

        //public static async Task<IEnumerable<TSource>> Reverse<TSource>(this Task<IEnumerable<TSource>> source)
        //{
        //}

        //public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        //{
        //}

        //public static bool SequenceEqual<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second, IEqualityComparer<TSource> comparer)
        //{
        //}

        public static async Task<IEnumerable<TSource>> AsEnumerable<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).AsEnumerable();
        }

        public static async Task<TSource[]> ToArray<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).ToArray();
        }

        public static async Task<List<TSource>> ToList<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).ToList();
        }

        #region ToDictionary

        public static async Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
        {
            return (await Cfg(source)).ToDictionary(keySelector);
        }

        public static async Task<Dictionary<TKey, TSource>> ToDictionary<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return (await Cfg(source)).ToDictionary(keySelector, comparer);
        }

        public static async Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return (await Cfg(source)).ToDictionary(keySelector, elementSelector);
        }

        public static async Task<Dictionary<TKey, TElement>> ToDictionary<TSource, TKey, TElement>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return (await Cfg(source)).ToDictionary(keySelector, elementSelector, comparer);
        }

        #endregion

        #region ToLookup

        public static async Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector)
        {
            return (await Cfg(source)).ToLookup(keySelector);
        }

        public static async Task<ILookup<TKey, TSource>> ToLookup<TSource, TKey>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer)
        {
            return (await Cfg(source)).ToLookup(keySelector, comparer);
        }

        public static async Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            return (await Cfg(source)).ToLookup(keySelector, elementSelector);
        }

        public static async Task<ILookup<TKey, TElement>> ToLookup<TSource, TKey, TElement>(this Task<IEnumerable<TSource>> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            return (await Cfg(source)).ToLookup(keySelector, elementSelector, comparer);
        }

        #endregion

        public static async Task<IEnumerable<TSource>> DefaultIfEmpty<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).DefaultIfEmpty();
        }

        public static async Task<IEnumerable<TSource>> DefaultIfEmpty<TSource>(this Task<IEnumerable<TSource>> source, TSource defaultValue)
        {
            return (await Cfg(source)).DefaultIfEmpty(defaultValue);
        }

        public static async Task<IEnumerable<TResult>> OfType<TResult>(this Task<IEnumerable> source)
        {
            return (await source.ConfigureAwait(false)).OfType<TResult>();
        }

        public static async Task<IEnumerable<TResult>> Cast<TResult>(this Task<IEnumerable> source)
        {
            return (await source.ConfigureAwait(false)).Cast<TResult>();
        }

        #region First/FirstOrDefault

        public static async Task<TSource> First<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).First();
        }

        public static async Task<TSource> First<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).First(predicate);
        }

        public static async Task<TSource> FirstOrDefault<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).FirstOrDefault();
        }

        public static async Task<TSource> FirstOrDefault<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).FirstOrDefault(predicate);
        }

        #endregion

        #region Last/LastOrDefault

        public static async Task<TSource> Last<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).Last();
        }

        public static async Task<TSource> Last<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).Last(predicate);
        }

        public static async Task<TSource> LastOrDefault<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).LastOrDefault();
        }

        public async static Task<TSource> LastOrDefault<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).LastOrDefault(predicate);
        }

        #endregion

        #region Single/SingleOrDefault

        public static async Task<TSource> Single<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).Single();
        }

        public static async Task<TSource> Single<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).Single(predicate);
        }

        public static async Task<TSource> SingleOrDefault<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).SingleOrDefault();
        }

        public static async Task<TSource> SingleOrDefault<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).SingleOrDefault(predicate);
        }

        #endregion

        public static async Task<TSource> ElementAt<TSource>(this Task<IEnumerable<TSource>> source, int index)
        {
            return (await Cfg(source)).ElementAt(index);
        }

        public static async Task<TSource> ElementAtOrDefault<TSource>(this Task<IEnumerable<TSource>> source, int index)
        {
            return (await Cfg(source)).ElementAtOrDefault(index);
        }

        public static async Task<bool> Any<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).Any();
        }

        public static async Task<bool> Any<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).Any(predicate);
        }

        public static async Task<bool> All<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).All(predicate);
        }

        public static async Task<int> Count<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).Count();
        }

        public static async Task<int> Count<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).Count(predicate);
        }

        public static async Task<long> LongCount<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).LongCount();
        }

        public static async Task<long> LongCount<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, bool> predicate)
        {
            return (await Cfg(source)).LongCount(predicate);
        }

        public static async Task<bool> Contains<TSource>(this Task<IEnumerable<TSource>> source, TSource value)
        {
            return (await Cfg(source)).Contains(value);
        }

        public static async Task<bool> Contains<TSource>(this Task<IEnumerable<TSource>> source, TSource value, IEqualityComparer<TSource> comparer)
        {
            return (await Cfg(source)).Contains(value, comparer);
        }

        public static async Task<TSource> Aggregate<TSource>(this Task<IEnumerable<TSource>> source, Func<TSource, TSource, TSource> func)
        {
            return (await Cfg(source)).Aggregate(func);
        }

        public static async Task<TAccumulate> Aggregate<TSource, TAccumulate>(this Task<IEnumerable<TSource>> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func)
        {
            return (await Cfg(source)).Aggregate(seed, func);
        }

        public static async Task<TResult> Aggregate<TSource, TAccumulate, TResult>(this Task<IEnumerable<TSource>> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func, Func<TAccumulate, TResult> resultSelector)
        {
            return (await Cfg(source)).Aggregate(seed, func, resultSelector);
        }

        public static async Task<TSource> Min<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).Min();
        }

        public async static Task<TResult> Min<TSource, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, TResult> selector)
        {
            return (await Cfg(source)).Min(selector);
        }

        public static async Task<TSource> Max<TSource>(this Task<IEnumerable<TSource>> source)
        {
            return (await Cfg(source)).Max();
        }

        public static async Task<TResult> Max<TSource, TResult>(this Task<IEnumerable<TSource>> source, Func<TSource, TResult> selector)
        {
            return (await Cfg(source)).Max(selector);
        }
    }
}
