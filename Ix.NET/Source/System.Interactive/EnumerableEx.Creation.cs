// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.
using System;
using System.Collections.Generic;

namespace System.Linq
{
    public static partial class EnumerableEx
    {
        /// <summary>
        /// Creates an enumerable sequence based on an enumerator factory function.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="getEnumerator">Enumerator factory function.</param>
        /// <returns>Sequence that will invoke the enumerator factory upon a call to GetEnumerator.</returns>
        public static IEnumerable<TResult> Create<TResult>(Func<IEnumerator<TResult>> getEnumerator)
        {
            if (getEnumerator == null)
                throw new ArgumentNullException("getEnumerator");

            return new AnonymousEnumerable<TResult>(getEnumerator);
        }

#if HAS_AWAIT
        public static IEnumerable<T> Create<T>(Action<IYielder<T>> create)
        {
            if (create == null)
                throw new ArgumentNullException("create");

            foreach (var x in new Yielder<T>(create))
                yield return x;
        }
#endif

        class AnonymousEnumerable<TResult> : IEnumerable<TResult>
        {
            private readonly Func<IEnumerator<TResult>> _getEnumerator;

            public AnonymousEnumerable(Func<IEnumerator<TResult>> getEnumerator)
            {
                _getEnumerator = getEnumerator;
            }

            public IEnumerator<TResult> GetEnumerator()
            {
                return _getEnumerator();
            }

            Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        /// <summary>
        /// Returns a sequence with a single element.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="value">Single element of the resulting sequence.</param>
        /// <returns>Sequence with a single element.</returns>
        public static IEnumerable<TResult> Return<TResult>(TResult value)
        {
            yield return value;
        }

        /// <summary>
        /// Returns a sequence that throws an exception upon enumeration.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="exception">Exception to throw upon enumerating the resulting sequence.</param>
        /// <returns>Sequence that throws the specified exception upon enumeration.</returns>
        public static IEnumerable<TResult> Throw<TResult>(Exception exception)
        {
            if (exception == null)
                throw new ArgumentNullException("exception");

            return Throw_<TResult>(exception);
        }

        private static IEnumerable<TResult> Throw_<TResult>(Exception exception)
        {
            throw exception;
#pragma warning disable 0162
            yield break;
#pragma warning restore 0162
        }

        /// <summary>
        /// Creates an enumerable sequence based on an enumerable factory function.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="enumerableFactory">Enumerable factory function.</param>
        /// <returns>Sequence that will invoke the enumerable factory upon a call to GetEnumerator.</returns>
        public static IEnumerable<TResult> Defer<TResult>(Func<IEnumerable<TResult>> enumerableFactory)
        {
            if (enumerableFactory == null)
                throw new ArgumentNullException("enumerableFactory");

            return Defer_(enumerableFactory);
        }

        private static IEnumerable<TSource> Defer_<TSource>(Func<IEnumerable<TSource>> enumerableFactory)
        {
            foreach (var item in enumerableFactory())
                yield return item;
        }

        /// <summary>
        /// Generates a sequence by mimicking a for loop.
        /// </summary>
        /// <typeparam name="TState">State type.</typeparam>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="initialState">Initial state of the generator loop.</param>
        /// <param name="condition">Loop condition.</param>
        /// <param name="iterate">State update function to run after every iteration of the generator loop.</param>
        /// <param name="resultSelector">Result selector to compute resulting sequence elements.</param>
        /// <returns>Sequence obtained by running the generator loop, yielding computed elements.</returns>
        public static IEnumerable<TResult> Generate<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            if (condition == null)
                throw new ArgumentNullException("condition");
            if (iterate == null)
                throw new ArgumentNullException("iterate");
            if (resultSelector == null)
                throw new ArgumentNullException("resultSelector");

            return Generate_(initialState, condition, iterate, resultSelector);
        }

        private static IEnumerable<TResult> Generate_<TState, TResult>(TState initialState, Func<TState, bool> condition, Func<TState, TState> iterate, Func<TState, TResult> resultSelector)
        {
            for (var i = initialState; condition(i); i = iterate(i))
                yield return resultSelector(i);
        }

        /// <summary>
        /// Generates a sequence that's dependent on a resource object whose lifetime is determined by the sequence usage duration.
        /// </summary>
        /// <typeparam name="TSource">Source element type.</typeparam>
        /// <typeparam name="TResource">Resource type.</typeparam>
        /// <param name="resourceFactory">Resource factory function.</param>
        /// <param name="enumerableFactory">Enumerable factory function, having access to the obtained resource.</param>
        /// <returns>Sequence whose use controls the lifetime of the associated obtained resource.</returns>
        public static IEnumerable<TSource> Using<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            if (resourceFactory == null)
                throw new ArgumentNullException("resourceFactory");
            if (enumerableFactory == null)
                throw new ArgumentNullException("enumerableFactory");

            return Using_(resourceFactory, enumerableFactory);
        }

        private static IEnumerable<TSource> Using_<TSource, TResource>(Func<TResource> resourceFactory, Func<TResource, IEnumerable<TSource>> enumerableFactory) where TResource : IDisposable
        {
            using (var res = resourceFactory())
                foreach (var item in enumerableFactory(res))
                    yield return item;
        }

        /// <summary>
        /// Generates a sequence by repeating the given value infinitely.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="value">Value to repreat in the resulting sequence.</param>
        /// <returns>Sequence repeating the given value infinitely.</returns>
        public static IEnumerable<TResult> Repeat<TResult>(TResult value)
        {
            while (true)
                yield return value;
        }

        /// <summary>
        /// Generates a sequence that contains one repeated value.
        /// </summary>
        /// <typeparam name="TResult">Result sequence element type.</typeparam>
        /// <param name="element">The value to be repeated.</param>
        /// <param name="count">The number of times to repeat the value in the generated sequence.</param>
        /// <returns>Sequence that contains a repeated value.</returns>
        public static IEnumerable<TResult> Repeat<TResult>(TResult element, int count)
        {
            return Enumerable.Repeat<TResult>(element, count);
        }
    }
}
