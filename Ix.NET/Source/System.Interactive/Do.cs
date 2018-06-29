// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        ///     Lazily invokes an action for each value in the sequence.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            return DoHelper(source, onNext, _ => { }, () => { });
        }

        /// <summary>
        ///     Lazily invokes an action for each value in the sequence, and executes an action for successful termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action onCompleted)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }

            return DoHelper(source, onNext, _ => { }, onCompleted);
        }

        /// <summary>
        ///     Lazily invokes an action for each value in the sequence, and executes an action upon exceptional termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            return DoHelper(source, onNext, onError, () => { });
        }

        /// <summary>
        ///     Lazily invokes an action for each value in the sequence, and executes an action upon successful or exceptional
        ///     termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="onNext">Action to invoke for each element.</param>
        /// <param name="onError">Action to invoke on exceptional termination of the sequence.</param>
        /// <param name="onCompleted">Action to invoke on successful termination of the sequence.</param>
        /// <returns>Sequence exhibiting the specified side-effects upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (onNext == null)
            {
                throw new ArgumentNullException(nameof(onNext));
            }

            if (onError == null)
            {
                throw new ArgumentNullException(nameof(onError));
            }

            if (onCompleted == null)
            {
                throw new ArgumentNullException(nameof(onCompleted));
            }

            return DoHelper(source, onNext, onError, onCompleted);
        }

        /// <summary>
        ///     Lazily invokes observer methods for each value in the sequence, and upon successful or exceptional termination.
        /// </summary>
        /// <typeparam name="TSource">Source sequence element type.</typeparam>
        /// <param name="source">Source sequence.</param>
        /// <param name="observer">Observer to invoke notification calls on.</param>
        /// <returns>Sequence exhibiting the side-effects of observer method invocation upon enumeration.</returns>
        public static IEnumerable<TSource> Do<TSource>(this IEnumerable<TSource> source, IObserver<TSource> observer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (observer == null)
            {
                throw new ArgumentNullException(nameof(observer));
            }

            return DoHelper(source, observer.OnNext, observer.OnError, observer.OnCompleted);
        }

        /// <summary>
        ///     Generates an enumerable sequence by repeating a source sequence as long as the given loop postcondition holds.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="source">Source sequence to repeat while the condition evaluates true.</param>
        /// <param name="condition">Loop condition.</param>
        /// <returns>Sequence generated by repeating the given sequence until the condition evaluates to false.</returns>
        public static IEnumerable<TResult> DoWhile<TResult>(this IEnumerable<TResult> source, Func<bool> condition)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            return source.Concat(While(condition, source));
        }

        private static IEnumerable<TSource> DoHelper<TSource>(this IEnumerable<TSource> source, Action<TSource> onNext, Action<Exception> onError, Action onCompleted)
        {
            using (var e = source.GetEnumerator())
            {
                while (true)
                {
                    var current = default(TSource);
                    try
                    {
                        if (!e.MoveNext())
                        {
                            break;
                        }

                        current = e.Current;
                    }
                    catch (Exception ex)
                    {
                        onError(ex);
                        throw;
                    }

                    onNext(current);
                    yield return current;
                }

                onCompleted();
            }
        }
    }
}