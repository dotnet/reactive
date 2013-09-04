// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

#if HAS_WINRT
using System.Reactive.Windows.Foundation;
using System.Threading;
using Windows.Foundation;

namespace System.Reactive.Linq
{
    public static partial class WindowsObservable
    {
        /// <summary>
        /// Projects each element of an observable sequence to a Windows Runtime asynchronous operation and merges all of the asynchronous operation results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the projected asynchronous operations and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence whose elements are the result of the asynchronous operations executed for each element of the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>This overload supports composition of observable sequences and Windows Runtime asynchronous operations, without requiring manual conversion of the asynchronous operations to observable sequences using <see cref="AsyncInfoObservableExtensions.ToObservable&lt;TResult&gt;(IAsyncOperation&lt;TResult&gt;)"/>.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IAsyncOperation<TResult>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.SelectMany(x => selector(x).ToObservable());
        }

        /// <summary>
        /// Projects each element of an observable sequence to a Windows Runtime asynchronous operation and merges all of the asynchronous operation results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the projected asynchronous operations and the elements in the merged result sequence.</typeparam>
        /// <typeparam name="TProgress">The type of the reported progress objects, which get ignored by this query operator.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence whose elements are the result of the asynchronous operations executed for each element of the input sequence.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>This overload supports composition of observable sequences and Windows Runtime asynchronous operations, without requiring manual conversion of the asynchronous operations to observable sequences using <see cref="AsyncInfoObservableExtensions.ToObservable&lt;TResult&gt;(IAsyncOperation&lt;TResult&gt;)"/>.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TResult, TProgress>(this IObservable<TSource> source, Func<TSource, IAsyncOperationWithProgress<TResult, TProgress>> selector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (selector == null)
                throw new ArgumentNullException("selector");

            return source.SelectMany(x => selector(x).ToObservable());
        }

        /// <summary>
        /// Projects each element of an observable sequence to a Windows Runtime asynchronous operation, invokes the result selector for the source element and the asynchronous operation result, and merges the results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TAsyncOperationResult">The type of the results produced by the projected asynchronous operations.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate asynchronous operation results.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="asyncOperationSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An observable sequence whose elements are the result of obtaining an asynchronous operation for each element of the input sequence and then mapping the asynchronous operation's result and its corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="asyncOperationSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and Windows Runtime asynchronous operations, without requiring manual conversion of the asynchronous operations to observable sequences using <see cref="AsyncInfoObservableExtensions.ToObservable&lt;TResult&gt;(IAsyncOperation&lt;TResult&gt;)"/>.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TAsyncOperationResult, TResult>(this IObservable<TSource> source, Func<TSource, IAsyncOperation<TAsyncOperationResult>> asyncOperationSelector, Func<TSource, TAsyncOperationResult, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (asyncOperationSelector == null)
                throw new ArgumentNullException("asyncOperationSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return source.SelectMany(x => asyncOperationSelector(x).ToObservable(), resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a Windows Runtime asynchronous operation, invokes the result selector for the source element and the asynchronous operation result, and merges the results into one observable sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TAsyncOperationResult">The type of the results produced by the projected asynchronous operations.</typeparam>
        /// <typeparam name="TAsyncOperationProgress">The type of the reported progress objects, which get ignored by this query operator.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate asynchronous operation results.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="asyncOperationSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An observable sequence whose elements are the result of obtaining an asynchronous operation for each element of the input sequence and then mapping the asynchronous operation's result and its corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="asyncOperationSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and Windows Runtime asynchronous operations, without requiring manual conversion of the asynchronous operations to observable sequences using <see cref="AsyncInfoObservableExtensions.ToObservable&lt;TResult&gt;(IAsyncOperation&lt;TResult&gt;)"/>.</remarks>
        public static IObservable<TResult> SelectMany<TSource, TAsyncOperationResult, TAsyncOperationProgress, TResult>(this IObservable<TSource> source, Func<TSource, IAsyncOperationWithProgress<TAsyncOperationResult, TAsyncOperationProgress>> asyncOperationSelector, Func<TSource, TAsyncOperationResult, TResult> resultSelector)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (asyncOperationSelector == null)
                throw new ArgumentNullException("asyncOperationSelector");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return source.SelectMany(x => asyncOperationSelector(x).ToObservable(), resultSelector);
        }
    }
}
#endif