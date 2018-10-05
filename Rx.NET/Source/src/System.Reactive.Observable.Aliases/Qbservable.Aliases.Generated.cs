/*
 * WARNING: Auto-generated file (05/28/2018 22:20:19)
 * Run Rx's auto-homoiconizer tool to generate this file (in the HomoIcon directory).
 */

#pragma warning disable 1591

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace System.Reactive.Linq
{
    [ExcludeFromCodeCoverage]
    public static partial class QbservableAliases
    {
        /// <summary>
        /// Filters the elements of an observable sequence based on a predicate.
        /// Synonym for the method 'Where'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to filter.</param>
        /// <param name="predicate">A function to test each source element for a condition.</param>
        /// <returns>An observable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        public static IQbservable<TSource> Filter<TSource>(this IQbservable<TSource> source, Expression<Func<TSource, bool>> predicate)
        {
            return Qbservable.Where<TSource>(source, predicate);
        }

        /// <summary>
        /// Filters the elements of an observable sequence based on a predicate by incorporating the element's index.
        /// Synonym for the method 'Where'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to filter.</param>
        /// <param name="predicate">A function to test each source element for a conditio; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="predicate" /> is null.</exception>
        public static IQbservable<TSource> Filter<TSource>(this IQbservable<TSource> source, Expression<Func<TSource, int, bool>> predicate)
        {
            return Qbservable.Where<TSource>(source, predicate);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an observable sequence, invokes the result selector for the source element and each of the corresponding inner sequence's elements, and merges the results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">The type of the elements in the projected intermediate sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate sequence elements.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="collectionSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function collectionSelector on each element of the input sequence and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> is null.</exception>
        public static IQbservable<TResult> FlatMap<TSource, TCollection, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, IObservable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            return Qbservable.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an observable sequence by incorporating the element's index, invokes the result selector for the source element and each of the corresponding inner sequence's elements, and merges the results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">The type of the elements in the projected intermediate sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate sequence elements.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="collectionSelector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence; the second parameter of the function represents the index of the source element and the fourth parameter represents the index of the intermediate element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function collectionSelector on each element of the input sequence and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> is null.</exception>
        public static IQbservable<TResult> FlatMap<TSource, TCollection, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, int, IObservable<TCollection>>> collectionSelector, Expression<Func<TSource, int, TCollection, int, TResult>> resultSelector)
        {
            return Qbservable.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an enumerable sequence, invokes the result selector for the source element and each of the corresponding inner sequence's elements, and merges the results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">The type of the elements in the projected intermediate enumerable sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate sequence elements.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="collectionSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function collectionSelector on each element of the input sequence and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="M:System.Reactive.Linq.Observable.ToObservable``1(System.Collections.Generic.IEnumerable{``0})" /> conversion.</remarks>
        public static IQbservable<TResult> FlatMap<TSource, TCollection, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, TCollection, TResult>> resultSelector)
        {
            return Qbservable.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an enumerable sequence by incorporating the element's index, invokes the result selector for the source element and each of the corresponding inner sequence's elements, and merges the results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TCollection">The type of the elements in the projected intermediate enumerable sequences.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate sequence elements.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="collectionSelector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence; the second parameter of the function represents the index of the source element and the fourth parameter represents the index of the intermediate element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function collectionSelector on each element of the input sequence and then mapping each of those sequence elements and their corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="collectionSelector" /> or <paramref name="resultSelector" /> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="M:System.Reactive.Linq.Observable.ToObservable``1(System.Collections.Generic.IEnumerable{``0})" /> conversion.</remarks>
        public static IQbservable<TResult> FlatMap<TSource, TCollection, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, int, IEnumerable<TCollection>>> collectionSelector, Expression<Func<TSource, int, TCollection, int, TResult>> resultSelector)
        {
            return Qbservable.SelectMany<TSource, TCollection, TResult>(source, collectionSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of the source observable sequence to the other observable sequence and merges the resulting observable sequences into one observable sequence. 
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TOther">The type of the elements in the other sequence and the elements in the result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="other">An observable sequence to project each element from the source sequence onto.</param>
        /// <returns>An observable sequence whose elements are the result of projecting each source element onto the other sequence and merging all the resulting sequences together.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="other" /> is null.</exception>
        public static IQbservable<TOther> FlatMap<TSource, TOther>(this IQbservable<TSource> source, IObservable<TOther> other)
        {
            return Qbservable.SelectMany<TSource, TOther>(source, other);
        }

        /// <summary>
        /// Projects each notification of an observable sequence to an observable sequence and merges the resulting observable sequences into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of notifications to project.</param>
        /// <param name="onNext">A transform function to apply to each element.</param>
        /// <param name="onError">A transform function to apply when an error occurs in the source sequence.</param>
        /// <param name="onCompleted">A transform function to apply when the end of the source sequence is reached.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function corresponding to each notification in the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="onNext" /> or <paramref name="onError" /> or <paramref name="onCompleted" /> is null.</exception>
        public static IQbservable<TResult> FlatMap<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, IObservable<TResult>>> onNext, Expression<Func<Exception, IObservable<TResult>>> onError, Expression<Func<IObservable<TResult>>> onCompleted)
        {
            return Qbservable.SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
        }

        /// <summary>
        /// Projects each notification of an observable sequence to an observable sequence by incorporating the element's index and merges the resulting observable sequences into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of notifications to project.</param>
        /// <param name="onNext">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="onError">A transform function to apply when an error occurs in the source sequence.</param>
        /// <param name="onCompleted">A transform function to apply when the end of the source sequence is reached.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function corresponding to each notification in the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="onNext" /> or <paramref name="onError" /> or <paramref name="onCompleted" /> is null.</exception>
        public static IQbservable<TResult> FlatMap<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, int, IObservable<TResult>>> onNext, Expression<Func<Exception, IObservable<TResult>>> onError, Expression<Func<IObservable<TResult>>> onCompleted)
        {
            return Qbservable.SelectMany<TSource, TResult>(source, onNext, onError, onCompleted);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an observable sequence and merges the resulting observable sequences into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IQbservable<TResult> FlatMap<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, IObservable<TResult>>> selector)
        {
            return Qbservable.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an observable sequence by incorporating the element's index and merges the resulting observable sequences into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IQbservable<TResult> FlatMap<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, int, IObservable<TResult>>> selector)
        {
            return Qbservable.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task and merges all of the task results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the projected tasks and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence whose elements are the result of the tasks executed for each element of the input sequence.</returns>
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="M:System.Reactive.Threading.Tasks.TaskObservableExtensions.ToObservable``1(System.Threading.Tasks.Task{``0})" />.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IQbservable<TResult> FlatMap<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, Task<TResult>>> selector)
        {
            return Qbservable.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task by incorporating the element's index and merges all of the task results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the projected tasks and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of the tasks executed for each element of the input sequence.</returns>
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="M:System.Reactive.Threading.Tasks.TaskObservableExtensions.ToObservable``1(System.Threading.Tasks.Task{``0})" />.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IQbservable<TResult> FlatMap<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, int, Task<TResult>>> selector)
        {
            return Qbservable.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task with cancellation support and merges all of the task results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the projected tasks and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence whose elements are the result of the tasks executed for each element of the input sequence.</returns>
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="M:System.Reactive.Threading.Tasks.TaskObservableExtensions.ToObservable``1(System.Threading.Tasks.Task{``0})" />.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IQbservable<TResult> FlatMap<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, CancellationToken, Task<TResult>>> selector)
        {
            return Qbservable.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task by incorporating the element's index with cancellation support and merges all of the task results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the result produced by the projected tasks and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of the tasks executed for each element of the input sequence.</returns>
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="M:System.Reactive.Threading.Tasks.TaskObservableExtensions.ToObservable``1(System.Threading.Tasks.Task{``0})" />.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IQbservable<TResult> FlatMap<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, int, CancellationToken, Task<TResult>>> selector)
        {
            return Qbservable.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an enumerable sequence and concatenates the resulting enumerable sequences into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner enumerable sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="M:System.Reactive.Linq.Observable.ToObservable``1(System.Collections.Generic.IEnumerable{``0})" /> conversion.</remarks>
        public static IQbservable<TResult> FlatMap<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, IEnumerable<TResult>>> selector)
        {
            return Qbservable.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to an enumerable sequence by incorporating the element's index and concatenates the resulting enumerable sequences into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the projected inner enumerable sequences and the elements in the merged result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="selector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the one-to-many transform function on each element of the input sequence.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="M:System.Reactive.Linq.Observable.ToObservable``1(System.Collections.Generic.IEnumerable{``0})" /> conversion.</remarks>
        public static IQbservable<TResult> FlatMap<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, int, IEnumerable<TResult>>> selector)
        {
            return Qbservable.SelectMany<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task, invokes the result selector for the source element and the task result, and merges the results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TTaskResult">The type of the results produced by the projected intermediate tasks.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate task results.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="taskSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An observable sequence whose elements are the result of obtaining a task for each element of the input sequence and then mapping the task's result and its corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="taskSelector" /> or <paramref name="resultSelector" /> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="M:System.Reactive.Threading.Tasks.TaskObservableExtensions.ToObservable``1(System.Threading.Tasks.Task{``0})" />.</remarks>
        public static IQbservable<TResult> FlatMap<TSource, TTaskResult, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, Task<TTaskResult>>> taskSelector, Expression<Func<TSource, TTaskResult, TResult>> resultSelector)
        {
            return Qbservable.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task by incorporating the element's index, invokes the result selector for the source element and the task result, and merges the results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TTaskResult">The type of the results produced by the projected intermediate tasks.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate task results.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="taskSelector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of obtaining a task for each element of the input sequence and then mapping the task's result and its corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="taskSelector" /> or <paramref name="resultSelector" /> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="M:System.Reactive.Threading.Tasks.TaskObservableExtensions.ToObservable``1(System.Threading.Tasks.Task{``0})" />.</remarks>
        public static IQbservable<TResult> FlatMap<TSource, TTaskResult, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, int, Task<TTaskResult>>> taskSelector, Expression<Func<TSource, int, TTaskResult, TResult>> resultSelector)
        {
            return Qbservable.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task with cancellation support, invokes the result selector for the source element and the task result, and merges the results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TTaskResult">The type of the results produced by the projected intermediate tasks.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate task results.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="taskSelector">A transform function to apply to each element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence.</param>
        /// <returns>An observable sequence whose elements are the result of obtaining a task for each element of the input sequence and then mapping the task's result and its corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="taskSelector" /> or <paramref name="resultSelector" /> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="M:System.Reactive.Threading.Tasks.TaskObservableExtensions.ToObservable``1(System.Threading.Tasks.Task{``0})" />.</remarks>
        public static IQbservable<TResult> FlatMap<TSource, TTaskResult, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, CancellationToken, Task<TTaskResult>>> taskSelector, Expression<Func<TSource, TTaskResult, TResult>> resultSelector)
        {
            return Qbservable.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence to a task by incorporating the element's index with cancellation support, invokes the result selector for the source element and the task result, and merges the results into one observable sequence.
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TTaskResult">The type of the results produced by the projected intermediate tasks.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by using the selector to combine source sequence elements with their corresponding intermediate task results.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="taskSelector">A transform function to apply to each element; the second parameter of the function represents the index of the source element.</param>
        /// <param name="resultSelector">A transform function to apply to each element of the intermediate sequence; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of obtaining a task for each element of the input sequence and then mapping the task's result and its corresponding source element to a result element.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="taskSelector" /> or <paramref name="resultSelector" /> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="M:System.Reactive.Threading.Tasks.TaskObservableExtensions.ToObservable``1(System.Threading.Tasks.Task{``0})" />.</remarks>
        public static IQbservable<TResult> FlatMap<TSource, TTaskResult, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, int, CancellationToken, Task<TTaskResult>>> taskSelector, Expression<Func<TSource, int, TTaskResult, TResult>> resultSelector)
        {
            return Qbservable.SelectMany<TSource, TTaskResult, TResult>(source, taskSelector, resultSelector);
        }

        /// <summary>
        /// Projects each element of an observable sequence into a new form. Synonym for the method 'Select'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam>
        /// <param name="source">A sequence of elements to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the transform function on each element of source.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IQbservable<TResult> Map<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, TResult>> selector)
        {
            return Qbservable.Select<TSource, TResult>(source, selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence into a new form by incorporating the element's index. Synonym for the method 'Select'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam>
        /// <param name="source">A sequence of elements to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the transform function on each element of source.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source" /> or <paramref name="selector" /> is null.</exception>
        public static IQbservable<TResult> Map<TSource, TResult>(this IQbservable<TSource> source, Expression<Func<TSource, int, TResult>> selector)
        {
            return Qbservable.Select<TSource, TResult>(source, selector);
        }

    }
}

#pragma warning restore 1591

