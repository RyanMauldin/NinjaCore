using System;
using System.Collections.Generic;
using System.Linq;

namespace NinjaCore.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Useful method that extends an IEnumerable sequence, which executes an action against each element in the sequence.
        /// http://stackoverflow.com/questions/1331931/net-foreach-extension-methods-and-dictionary
        /// https://blogs.msdn.microsoft.com/ericlippert/2009/05/18/foreach-vs-foreach/
        /// </summary>
        /// <typeparam name="T">Type of element in sequence.</typeparam>
        /// <param name="sequence">The IEnumerable sequence.</param>
        /// <param name="action">The action to execute on each element in the sequence.</param>
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (action == null) throw new ArgumentNullException(nameof(action));
            foreach (var item in sequence) action(item);
        }

        /// <summary>
        /// Useful method that extends an IEnumerable sequence, which executes a function against each element in the sequence.
        /// Returning false in any of the functions stops iteration.
        /// http://stackoverflow.com/questions/1331931/net-foreach-extension-methods-and-dictionary
        /// https://blogs.msdn.microsoft.com/ericlippert/2009/05/18/foreach-vs-foreach/
        /// </summary>
        /// <typeparam name="T">Type of element in sequence.</typeparam>
        /// <param name="sequence">The IEnumerable sequence.</param>
        /// <param name="function"></param>
        public static void ForEach<T>(this IEnumerable<T> sequence, Func<T, bool> function)
        {
            if (sequence == null) throw new ArgumentNullException(nameof(sequence));
            if (function == null) throw new ArgumentNullException(nameof(function));
            if (sequence.Any(item => !function(item))) return;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> array)
        {
            return new HashSet<T>(array);
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> array, IEqualityComparer<T> comparer)
        {
            return new HashSet<T>(array, comparer);
        }

        public static HashSet<TOut> ToHashSet<TIn, TOut>(this IEnumerable<TIn> array, Func<TIn, TOut> predicate)
        {
            return new HashSet<TOut>(array.Select(predicate));
        }

        public static HashSet<TOut> ToHashSet<TIn, TOut>(this IEnumerable<TIn> array, Func<TIn, TOut> predicate, IEqualityComparer<TOut> comparer)
        {
            return new HashSet<TOut>(array.Select(predicate), comparer);
        }
    }
}
