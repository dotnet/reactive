// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the Apache 2.0 License.
// See the LICENSE file in the project root for more information. 

#pragma warning disable 1591

using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks; // needed for doc comments
using System.Threading;
using System.Threading.Tasks;

/*
 * Note: these methods just call methods in Observable.StandardSequenceOperators.cs
 * in order to create the following method aliases:
 * 
 * Map     = Select
 * FlatMap = SelectMany
 * Filter  = Where
 * 
 */

namespace System.Reactive.Observable.Aliases
{
    public static class QueryLanguage
    {
        #region + Map +

        /// <summary>
        /// Projects each element of an observable sequence into a new form. Synonym for the method 'Select'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam>
        /// <param name="source">A sequence of elements to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the transform function on each element of source.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> Map<TSource, TResult>(this IObservable<TSource> source, Func<TSource, TResult> selector)
        {
            return source.Select<TSource, TResult>(selector);
        }

        /// <summary>
        /// Projects each element of an observable sequence into a new form by incorporating the element's index. Synonym for the method 'Select'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TResult">The type of the elements in the result sequence, obtained by running the selector function for each element in the source sequence.</typeparam>
        /// <param name="source">A sequence of elements to invoke a transform function on.</param>
        /// <param name="selector">A transform function to apply to each source element; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence whose elements are the result of invoking the transform function on each element of source.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> Map<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, TResult> selector)
        {
            return source.Select<TSource, TResult>(selector);
        }

        #endregion

        #region + FlatMap +

        /// <summary>
        /// Projects each element of the source observable sequence to the other observable sequence and merges the resulting observable sequences into one observable sequence. 
        /// Synonym for the method 'SelectMany'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <typeparam name="TOther">The type of the elements in the other sequence and the elements in the result sequence.</typeparam>
        /// <param name="source">An observable sequence of elements to project.</param>
        /// <param name="other">An observable sequence to project each element from the source sequence onto.</param>
        /// <returns>An observable sequence whose elements are the result of projecting each source element onto the other sequence and merging all the resulting sequences together.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="other"/> is null.</exception>
        public static IObservable<TOther> FlatMap<TSource, TOther>(this IObservable<TSource> source, IObservable<TOther> other)
        {
            return source.SelectMany<TSource, TOther>(other);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> FlatMap<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TResult>> selector)
        {
            return source.SelectMany<TSource, TResult>(selector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> FlatMap<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> selector)
        {
            return source.SelectMany<TSource, TResult>(selector);
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
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable{TResult}(Task{TResult})"/>.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> FlatMap<TSource, TResult>(this IObservable<TSource> source, Func<TSource, Task<TResult>> selector)
        {
            return source.SelectMany<TSource, TResult>(selector);
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
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable{TResult}(Task{TResult})"/>.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> FlatMap<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, Task<TResult>> selector)
        {
            return source.SelectMany<TSource, TResult>(selector);
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
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable{TResult}(Task{TResult})"/>.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> FlatMap<TSource, TResult>(this IObservable<TSource> source, Func<TSource, CancellationToken, Task<TResult>> selector)
        {
            return source.SelectMany<TSource, TResult>(selector);
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
        /// <remarks>This overload supports composition of observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable{TResult}(Task{TResult})"/>.</remarks>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        public static IObservable<TResult> FlatMap<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TResult>> selector)
        {
            return source.SelectMany<TSource, TResult>(selector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="collectionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> FlatMap<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return source.SelectMany<TSource, TCollection, TResult>(collectionSelector, resultSelector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="collectionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        public static IObservable<TResult> FlatMap<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            return source.SelectMany<TSource, TCollection, TResult>(collectionSelector, resultSelector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="taskSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable{TResult}(Task{TResult})"/>.</remarks>
        public static IObservable<TResult> FlatMap<TSource, TTaskResult, TResult>(this IObservable<TSource> source, Func<TSource, Task<TTaskResult>> taskSelector, Func<TSource, TTaskResult, TResult> resultSelector)
        {
            return source.SelectMany<TSource, TTaskResult, TResult>(taskSelector, resultSelector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="taskSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable{TResult}(Task{TResult})"/>.</remarks>
        public static IObservable<TResult> FlatMap<TSource, TTaskResult, TResult>(this IObservable<TSource> source, Func<TSource, int, Task<TTaskResult>> taskSelector, Func<TSource, int, TTaskResult, TResult> resultSelector)
        {
            return source.SelectMany<TSource, TTaskResult, TResult>(taskSelector, resultSelector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="taskSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable{TResult}(Task{TResult})"/>.</remarks>
        public static IObservable<TResult> FlatMap<TSource, TTaskResult, TResult>(this IObservable<TSource> source, Func<TSource, CancellationToken, Task<TTaskResult>> taskSelector, Func<TSource, TTaskResult, TResult> resultSelector)
        {
            return source.SelectMany<TSource, TTaskResult, TResult>(taskSelector, resultSelector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="taskSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>This overload supports using LINQ query comprehension syntax in C# and Visual Basic to compose observable sequences and tasks, without requiring manual conversion of the tasks to observable sequences using <see cref="TaskObservableExtensions.ToObservable{TResult}(Task{TResult})"/>.</remarks>
        public static IObservable<TResult> FlatMap<TSource, TTaskResult, TResult>(this IObservable<TSource> source, Func<TSource, int, CancellationToken, Task<TTaskResult>> taskSelector, Func<TSource, int, TTaskResult, TResult> resultSelector)
        {
            return source.SelectMany<TSource, TTaskResult, TResult>(taskSelector, resultSelector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is null.</exception>
        public static IObservable<TResult> FlatMap<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IObservable<TResult>> onNext, Func<Exception, IObservable<TResult>> onError, Func<IObservable<TResult>> onCompleted)
        {
            return source.SelectMany<TSource, TResult>(onNext, onError, onCompleted);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="onNext"/> or <paramref name="onError"/> or <paramref name="onCompleted"/> is null.</exception>
        public static IObservable<TResult> FlatMap<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IObservable<TResult>> onNext, Func<Exception, IObservable<TResult>> onError, Func<IObservable<TResult>> onCompleted)
        {
            return source.SelectMany<TSource, TResult>(onNext, onError, onCompleted);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="Linq.Observable.ToObservable{TSource}(IEnumerable{TSource})"/> conversion.</remarks>
        public static IObservable<TResult> FlatMap<TSource, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TResult>> selector)
        {
            return source.SelectMany<TSource, TResult>(selector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="selector"/> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="Linq.Observable.ToObservable{TSource}(IEnumerable{TSource})"/> conversion.</remarks>
        public static IObservable<TResult> FlatMap<TSource, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TResult>> selector)
        {
            return source.SelectMany<TSource, TResult>(selector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="collectionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="Linq.Observable.ToObservable{TSource}(IEnumerable{TSource})"/> conversion.</remarks>
        public static IObservable<TResult> FlatMap<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, IEnumerable<TCollection>> collectionSelector, Func<TSource, TCollection, TResult> resultSelector)
        {
            return source.SelectMany<TSource, TCollection, TResult>(collectionSelector, resultSelector);
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
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="collectionSelector"/> or <paramref name="resultSelector"/> is null.</exception>
        /// <remarks>The projected sequences are enumerated synchonously within the OnNext call of the source sequence. In order to do a concurrent, non-blocking merge, change the selector to return an observable sequence obtained using the <see cref="Linq.Observable.ToObservable{TSource}(IEnumerable{TSource})"/> conversion.</remarks>
        public static IObservable<TResult> FlatMap<TSource, TCollection, TResult>(this IObservable<TSource> source, Func<TSource, int, IEnumerable<TCollection>> collectionSelector, Func<TSource, int, TCollection, int, TResult> resultSelector)
        {
            return source.SelectMany<TSource, TCollection, TResult>(collectionSelector, resultSelector);
        }

        #endregion

        #region + Filter +

        /// <summary>
        /// Filters the elements of an observable sequence based on a predicate.
        /// Synonym for the method 'Where'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to filter.</param>
        /// <param name="predicate">A function to test each source element for a condition.</param>
        /// <returns>An observable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IObservable<TSource> Filter<TSource>(this IObservable<TSource> source, Func<TSource, bool> predicate)
        {
            return source.Where<TSource>(predicate);
        }

        /// <summary>
        /// Filters the elements of an observable sequence based on a predicate by incorporating the element's index.
        /// Synonym for the method 'Where'
        /// </summary>
        /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
        /// <param name="source">An observable sequence whose elements to filter.</param>
        /// <param name="predicate">A function to test each source element for a conditio; the second parameter of the function represents the index of the source element.</param>
        /// <returns>An observable sequence that contains elements from the input sequence that satisfy the condition.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> or <paramref name="predicate"/> is null.</exception>
        public static IObservable<TSource> Filter<TSource>(this IObservable<TSource> source, Func<TSource, int, bool> predicate)
        {
            return source.Where<TSource>(predicate);
        }

        #endregion
    }

}

#pragma warning restore 1591
